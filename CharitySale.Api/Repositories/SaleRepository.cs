using CharitySale.Api.Context;
using CharitySale.Api.Entities;

namespace CharitySale.Api.Repositories;

public class SaleRepository(CharitySaleDbContext context) : BaseRepository<Sale>(context), ISaleRepository
{
    
}