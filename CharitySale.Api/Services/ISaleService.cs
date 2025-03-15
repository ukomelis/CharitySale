using CharitySale.Api.Entities;
using CharitySale.Api.Models;
using CharitySale.Api.Models.DTOs;

namespace CharitySale.Api.Services;

public interface ISaleService
{
    Task<Result<IEnumerable<Sale>>> GetAllSalesAsync();
    Task<Result<Sale>> GetSaleByIdAsync(Guid id);
    Task<Result<Sale>> CreateSaleAsync(CreateSaleDto saleDto);
    Task<Result<bool>> DeleteSaleAsync(Guid id);
}