using CharitySale.Api.Entities;

namespace CharitySale.Api.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<IEnumerable<Sale>> GetAllWithItemsAsync();
    Task<Sale?> GetByIdWithItemsAsync(Guid id);
}