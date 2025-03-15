using System.Linq.Expressions;
using CharitySale.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api.Repositories;

public class BaseRepository<T>(CharitySaleDbContext context) : IRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        
        return entity;
    }

    public virtual T Update(T entity)
    {
        _dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        
        return entity;
    }

    public virtual void Delete(T entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> SaveChangesAsync()
    {
       return await context.SaveChangesAsync() > 0;
    }
}