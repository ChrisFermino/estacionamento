using Infrastructure.Models;
using Infrastructure.Models.DTO;

namespace controle_de_estacionamento.Services.IServices;

public interface IParkingService
{
    Task<List<Parking>> GetAllAsync();

    Task<Parking?> GetByIdAsync(long id);

    Task<Parking> CheckIn(CheckInDTO entity);

    Task<Parking> CheckOut(CheckOutDTO entity);

    Task<Parking> DeleteAsync(long id);
}
