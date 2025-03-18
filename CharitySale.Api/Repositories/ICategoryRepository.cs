using CharitySale.Api.Entities;

namespace CharitySale.Api.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetCategoryWithItemsAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesWithItemsAsync();
}