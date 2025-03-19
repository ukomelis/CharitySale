using CharitySale.Shared.Models;

namespace CharitySale.Web.Services;

public interface ISaleService
{
    Task<Sale> CreateSaleAsync(CreateSale sale);
    Task<List<Sale>> GetAllSalesAsync();
    Task<Sale> GetSaleByIdAsync(Guid id);
}