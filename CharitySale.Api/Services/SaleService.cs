using AutoMapper;
using CharitySale.Api.Models;
using CharitySale.Api.Repositories;
using CharitySale.Shared.Models;
using Sale = CharitySale.Api.Entities.Sale;

namespace CharitySale.Api.Services;

public class SaleService(ISaleRepository saleRepository, IItemService itemService, ILogger<SaleService> logger) : ISaleService
{
    public async Task<Result<IEnumerable<Sale>>> GetAllSalesAsync()
    {
        try
        {
            var sales = await saleRepository.GetAllWithItemsAsync();
            return Result<IEnumerable<Sale>>.Success(sales);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all sales");
            return Result<IEnumerable<Sale>>.Failure("Failed to retrieve sales from database.");
        }
    }

    public async Task<Result<Sale>> GetSaleByIdAsync(Guid id)
    {
        try
        {
            var sale = await saleRepository.GetByIdWithItemsAsync(id);
            return sale == null ? Result<Sale>.Failure($"Sale with ID {id} not found.") : Result<Sale>.Success(sale);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving sale {Id}", id);
            return Result<Sale>.Failure($"Failed to retrieve sale {id} from database.");
        }
    }

    public async Task<Result<Sale>> CreateSaleAsync(CreateSale request)
    {
        try
        {
            // Validate items exist and have sufficient stock
            var itemValidationResults = await ValidateItemsAvailabilityAsync(request.Items);
            if (!itemValidationResults.IsSuccess)
            {
                return Result<Sale>.Failure(itemValidationResults.Error!);
            }

            // Calculate total
            var total = await CalculateTotalAsync(request.Items);

            // Validate payment
            if (request.AmountPaid < total)
            {
                return Result<Sale>.Failure($"Insufficient payment. Required: {total:C}, Provided: {request.AmountPaid:C}");
            }

            // Create sale entity
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleDate = request.SaleDate,
                TotalAmount = total,
                SaleItems = new List<Entities.SaleItem>()
            };

            // Add items to sale and update inventory
            foreach (var item in request.Items)
            {
                var result = await itemService.UpdateItemStockAsync(item.ItemId, -item.Quantity);
                if (!result.IsSuccess)
                {
                    // In a real application, you might want to implement transaction rollback here
                    return Result<Sale>.Failure($"Failed to update inventory for item {item.ItemId}");
                }

                var saleItem = new Entities.SaleItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = result.Value!.Price,
                    Sale = sale,
                    Item = result.Value
                };

                sale.SaleItems.Add(saleItem);
            }

            // Save sale
            await saleRepository.AddAsync(sale);
            await saleRepository.SaveChangesAsync();

            return Result<Sale>.Success(sale);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating sale {@CreateSale}", request);
            return Result<Sale>.Failure("Failed to create sale in database.");
        }
    }

    public async Task<Result<bool>> DeleteSaleAsync(Guid id)
    {
        try
        {
            var sale = await saleRepository.GetByIdAsync(id);
            if (sale == null)
            {
                return Result<bool>.Failure($"Sale with ID {id} not found.");
            }

            saleRepository.Delete(sale);
            await saleRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting sale {Id}", id);
            return Result<bool>.Failure($"Failed to delete sale {id} from database.");
        }
    }

    private async Task<Result<bool>> ValidateItemsAvailabilityAsync(IEnumerable<CreateSaleItem> items)
    {
        foreach (var item in items)
        {
            var itemResult = await itemService.GetItemByIdAsync(item.ItemId);
            if (!itemResult.IsSuccess)
            {
                return Result<bool>.Failure($"Item {item.ItemId} not found.");
            }

            if (itemResult.Value!.Quantity < item.Quantity)
            {
                return Result<bool>.Failure(
                    $"Insufficient stock for item {itemResult.Value.Name}. " +
                    $"Requested: {item.Quantity}, Available: {itemResult.Value.Quantity}");
            }
        }

        return Result<bool>.Success(true);
    }

    private async Task<decimal> CalculateTotalAsync(IEnumerable<CreateSaleItem> items)
    {
        decimal total = 0;
        foreach (var item in items)
        {
            var itemResult = await itemService.GetItemByIdAsync(item.ItemId);
            if (itemResult.IsSuccess)
            {
                total += itemResult.Value!.Price * item.Quantity;
            }
        }
        return total;
    }
}