using CharitySale.Api.Entities;

namespace CharitySale.Api.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Task<IEnumerable<Item>> GetItemsByCategoryIdAsync(int categoryId);
    Task<Item?> GetItemWithCategoryAsync(Guid id);

}