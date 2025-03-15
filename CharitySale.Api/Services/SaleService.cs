using CharitySale.Api.Entities;
using CharitySale.Api.Models;
using CharitySale.Api.Models.DTOs;
using CharitySale.Api.Repositories;

namespace CharitySale.Api.Services;

public class SaleService(ISaleRepository saleRepository) : ISaleService
{
    public async Task<Result<IEnumerable<Sale>>> GetAllSalesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Sale>> GetSaleByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Sale>> CreateSaleAsync(CreateSaleDto saleDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteSaleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}