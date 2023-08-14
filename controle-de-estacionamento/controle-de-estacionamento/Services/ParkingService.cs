using controle_de_estacionamento.Repositories.Interfaces;
using controle_de_estacionamento.Services.IServices;
using Infrastructure.Models;
using Infrastructure.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace controle_de_estacionamento.Services;

public class ParkingService : IParkingService
{
    private readonly IParkingRepository parkingRepository;
    private readonly IPriceService priceService;

    public ParkingService(IParkingRepository parkingRepository, IPriceService priceService)
    {
        this.parkingRepository = parkingRepository;
        this.priceService = priceService;
    }

    public async Task<List<Parking>> GetAllAsync()
    {
        return await this.parkingRepository.GetAllAsync();
    }

    public async Task<Parking?> GetByIdAsync(long id)
    {
        return await this.parkingRepository.GetByIdAsync(id);
    }

    private Task<Parking> GetVehicleByPlate(string plate)
    {
        return this.parkingRepository.GetDbSet()
            .FirstOrDefaultAsync(
            vehicle => vehicle.Plate == plate && vehicle.IsInsideParking && vehicle.DeleteDate == null);
    }

    public async Task<Parking> CheckIn(CheckInDTO entity)
    {
        Price activePrice = priceService.GetActivePrice(entity.CheckInDate);

        var existingVehicle = await this.parkingRepository.GetDbSet()
            .FirstOrDefaultAsync(vehicle => vehicle.Plate == entity.Plate && vehicle.IsInsideParking);

        if (existingVehicle != null) throw new BadHttpRequestException("The vehicle is already in the parking lot");

        if (activePrice == null) throw new BadHttpRequestException("No active pricing table found for entry date");

        return await this.parkingRepository.CreateAsync(new Parking(entity));
    }

    public async Task<Parking> CheckOut(CheckOutDTO entity)
    {
        Price activePrice = this.priceService.GetActivePrice(entity.CheckOutDate);

        if (activePrice == null) throw new BadHttpRequestException("No active pricing table found for entry date");

        var vehicle = await this.GetVehicleByPlate(entity.Plate);

        if (vehicle == null) throw new BadHttpRequestException("Vehicle not found");

        vehicle.CheckOutDate = entity.CheckOutDate;
        vehicle.IsInsideParking = false;

        (int totalMinutes, int chargedTime) = CalculateDurationAndChargedTime(vehicle.CheckInDate, entity.CheckOutDate);

        decimal amountCharged = this.priceService.ParkingFeeCalculation(chargedTime, totalMinutes, activePrice);

        vehicle.HoursDuration = (int)totalMinutes / 60;
        vehicle.MinutesDuration = (int)totalMinutes % 60;
        vehicle.ChargedTime = chargedTime;
        vehicle.AmountCharged = amountCharged;

        await parkingRepository.EditAsync(vehicle);

        return vehicle;
    }

    public async Task<Parking> DeleteAsync(long id)
    {
        return await this.parkingRepository.DeleteAsync(id);
    }

    private (int totalMinutes, int chargetTime) CalculateDurationAndChargedTime(DateTime checkInDate, DateTime checkOutDate)
    {
        if (checkOutDate < checkInDate) throw new BadHttpRequestException("The output date must be greater than the input date!");

        TimeSpan duration = checkOutDate - checkInDate;

        int durationMinutes = (int)duration.TotalMinutes;
        int totalHours = durationMinutes / 60;
        int minutes = durationMinutes % 60;

        int chargedHours;

        if (durationMinutes < 10 && totalHours < 1) chargedHours = 30;

        if (totalHours >= 1 && minutes > 10)
        {
            chargedHours = (int)Math.Ceiling((double)durationMinutes / 60);
        }
        else
        {
            chargedHours = totalHours;
        }
        return (durationMinutes, chargedHours);
    }
}
