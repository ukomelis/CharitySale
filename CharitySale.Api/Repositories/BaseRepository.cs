using System.Linq.Expressions;
using CharitySale.Api.Context;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api.Repositories;

public class BaseRepository<T>(CharitySaleDbContext context) : IRepository<T>
    where T : class
{
    protected readonly CharitySaleDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> SaveChangesAsync()
    {
       return await _context.SaveChangesAsync() > 0;
    }
}