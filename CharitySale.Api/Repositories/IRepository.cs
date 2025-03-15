using System.Linq.Expressions;

namespace CharitySale.Api.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    T Update(T entity);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
}