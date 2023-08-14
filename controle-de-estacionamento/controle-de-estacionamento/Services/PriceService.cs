using controle_de_estacionamento.Repositories.Interfaces;
using controle_de_estacionamento.Services.IServices;
using Infrastructure.Models;

namespace controle_de_estacionamento.Services;

public class PriceService : IPriceService
{

    public readonly IPriceRepository priceRepository;

    public PriceService(IPriceRepository priceRepository)
    {
        this.priceRepository = priceRepository;
    }

    public async Task<List<Price>> GetAllAsync()
    {
        return await this.priceRepository.GetAllAsync();
    }

    public async Task<Price> GetByIdAsync(long id)
    {
        return await this.priceRepository.GetByIdAsync(id);
    }

    public Price GetActivePrice(DateTime date)
    {
        return this.priceRepository.GetAllAsync().Result.FirstOrDefault(p => p.IsActive && p.StartDate.Date <= date.Date && p.EndDate.Date >= date);
    }

    public async Task<Price> CreateAsync(Price entity)
    {
        var prices = await this.priceRepository.GetAllAsync();
        var hasPrices = prices.Any(p => p.StartDate <= entity.StartDate && p.EndDate >= entity.EndDate);

        if (hasPrices)
        {
            throw new BadHttpRequestException("There is already a price table registered for the same period!");
        }
        else
        {
            return await this.priceRepository.CreateAsync(entity);
        }
    }

    public async Task<Price> EditAsync(Price entity)
    {
        return await priceRepository.EditAsync(entity);
    }

    public async Task<Price> DeleteAsync(long id)
    {
        return await priceRepository.DeleteAsync(id);
    }

    public decimal ParkingFeeCalculation(int chargedTime, int totalMinutes, Price price)
    {
        var StartTimeValue = price.StartTimeValue;
        var AdditionalFeePerHour = price.AdditionalFeePerHour;

        if (totalMinutes <= 30)
        {
            return StartTimeValue / 2;
        }

        decimal totalCharge = StartTimeValue;

        for (int i = 1; i < chargedTime; i++)
        {
            totalCharge += AdditionalFeePerHour;
        }

        return totalCharge;
    }
}
