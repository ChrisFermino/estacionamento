using controle_de_estacionamento.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Models.Interfaces;
using Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace controle_de_estacionamento.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IBase
{
    private readonly Context context;
    public BaseRepository(Context context)
    {
        this.context = context;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await this.context.Set<T>().Where(x => x.DeleteDate == null).ToListAsync();
    }

    public async Task<T> GetByIdAsync(long id)
    {
        dynamic result = await this.context.Set<T>().FindAsync(id);

        if (result == null || result.DeleteDate != null)
        {
            throw new KeyNotFoundException("Register not found!");
        }

        return result;
    }

    public async Task<T> CreateAsync(T entity)
    {
        entity.CreateDate = DateTime.Now;

        this.context.Set<T>().Add(entity);
        await this.context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> EditAsync(T entity)
    {
        T existingEntity = (T)this.context.Find(entity.GetType(), entity.Id);
        if (existingEntity == null)
        {
            throw new KeyNotFoundException("Register not found!");
        }

        Utils.CopyObjectToAnother(entity, existingEntity);

        existingEntity.UpdateDate = DateTime.Now;

        this.context.Entry(existingEntity).State = EntityState.Modified;
        await this.context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(long id)
    {
        dynamic result = await this.context.Set<T>().FindAsync(id);

        if (result == null || result.DeleteDate != null)
        {
            throw new KeyNotFoundException("Register not found!");
        }

        result.DeleteDate = DateTime.Now;

        this.context.Set<T>().Update(result);
        await this.context.SaveChangesAsync();
        return result;
    }
    public DbSet<T> GetDbSet()
    {
        return context.Set<T>();
    }
}