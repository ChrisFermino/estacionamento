using Infrastructure.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace controle_de_estacionamento.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class, IBase
{
    Task<List<T>> GetAllAsync();

    Task<T> GetByIdAsync(long id);

    Task<T> CreateAsync(T entity);

    Task<T> EditAsync(T entity);

    Task<T> DeleteAsync(long id);

    DbSet<T> GetDbSet();
}
