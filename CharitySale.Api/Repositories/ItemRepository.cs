using CharitySale.Api.Context;
using CharitySale.Api.Entities;

namespace CharitySale.Api.Repositories;

public class ItemRepository(CharitySaleDbContext context) : BaseRepository<Item>(context), IItemRepository
{
    
}