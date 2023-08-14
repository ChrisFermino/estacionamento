using Infrastructure.Models;

namespace controle_de_estacionamento.Services.IServices;

public interface IPriceService
{
    Task<List<Price>> GetAllAsync();

    Task<Price> GetByIdAsync(long id);

    Price GetActivePrice(DateTime date);

    Task<Price> CreateAsync(Price entity);

    Task<Price> EditAsync(Price entity);

    Task<Price> DeleteAsync(long id);

    decimal ParkingFeeCalculation(int chargedTime, int totalMinutes, Price price);
}
