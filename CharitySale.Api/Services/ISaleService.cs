using CharitySale.Api.Models;
using CharitySale.Shared.Models;

namespace CharitySale.Api.Services;

public interface ISaleService
{
    Task<Result<IEnumerable<Sale>>> GetAllSalesAsync();
    Task<Result<Sale>> GetSaleByIdAsync(Guid id);
    Task<Result<Sale>> CreateSaleAsync(CreateSale sale);
    Task<Result<bool>> ValidateItemsAvailabilityAsync(IEnumerable<CreateSaleItem> items);
    Task<decimal> CalculateTotalAsync(IEnumerable<CreateSaleItem> items);
    List<Change> CalculateChangeDenominations(decimal amount);
}