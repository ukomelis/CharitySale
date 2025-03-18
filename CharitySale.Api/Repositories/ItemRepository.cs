using CharitySale.Api.Context;
using CharitySale.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api.Repositories;

public class ItemRepository(CharitySaleDbContext context) : BaseRepository<Item>(context), IItemRepository
{
    private readonly CharitySaleDbContext _context = context;
    
    public override async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _context.Items
            .Include(i => i.Category)  // Include the Category navigation property
            .ToListAsync();
    }

    public async Task<IEnumerable<Item>> GetItemsByCategoryIdAsync(int categoryId)
    {
        return await _context.Items
            .Where(i => i.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Item?> GetItemWithCategoryAsync(Guid id)
    {
        return await _context.Items
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}