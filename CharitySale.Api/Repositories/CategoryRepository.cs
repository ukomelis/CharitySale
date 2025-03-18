using CharitySale.Api.Context;
using CharitySale.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api.Repositories;

public class CategoryRepository(CharitySaleDbContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    private readonly CharitySaleDbContext _context = context;

    public async Task<Category?> GetCategoryWithItemsAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetCategoriesWithItemsAsync()
    {
        return await _context.Categories
            .Include(c => c.Items)
            .ToListAsync();
    }
}