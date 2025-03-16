using CharitySale.Api.Context;
using CharitySale.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api.Repositories;

public class SaleRepository(CharitySaleDbContext context) : BaseRepository<Sale>(context), ISaleRepository
{
    private readonly CharitySaleDbContext _context = context;

    public async Task<IEnumerable<Sale>> GetAllWithItemsAsync()
    {
        return await _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Item)
            .ToListAsync();
    }
    
    public async Task<Sale?> GetByIdWithItemsAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Item)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}