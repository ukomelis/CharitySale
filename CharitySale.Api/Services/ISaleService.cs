using CharitySale.Api.Models;
using CharitySale.Shared.Models;
using Sale = CharitySale.Api.Entities.Sale;

namespace CharitySale.Api.Services;

public interface ISaleService
{
    Task<Result<IEnumerable<Sale>>> GetAllSalesAsync();
    Task<Result<Sale>> GetSaleByIdAsync(Guid id);
    Task<Result<Sale>> CreateSaleAsync(CreateSale sale);
    Task<Result<bool>> DeleteSaleAsync(Guid id);
}