using AutoMapper;
using CharitySale.Api.Models;
using CharitySale.Api.Repositories;
using CharitySale.Shared.Models;

namespace CharitySale.Api.Services;

public class SaleService(
    ISaleRepository saleRepository,
    IItemRepository itemRepository,
    IMapper mapper,
    ILogger<SaleService> logger)
    : ISaleService
{
    public async Task<Result<Sale>> CreateSaleAsync(CreateSale request)
    {
        // Validate items availability
        var availabilityResult = await ValidateItemsAvailabilityAsync(request.Items);
        if (availabilityResult is { IsSuccess: false, Error: not null }) return Result<Sale>.Failure(availabilityResult.Error);

        // Calculate total
        var totalAmount = await CalculateTotalAsync(request.Items);
        
        // Validate payment
        if (request.AmountPaid < totalAmount)
        {
            return Result<Sale>.Failure(
                $"Insufficient payment. Required: {totalAmount:C}, Received: {request.AmountPaid:C}");
        }

        // Create sale entity
        var saleEntity = new Entities.Sale
        {
            SaleDate = request.SaleDate == default ? DateTime.UtcNow : request.SaleDate,
            TotalAmount = totalAmount
        };

        try
        {
            // Process items and update inventory
            foreach (var item in request.Items)
            {
                var itemEntity = await itemRepository.GetByIdAsync(item.ItemId);
                if (itemEntity == null)
                {
                    return Result<Sale>.Failure($"Item {item.ItemId} not found");
                }

                saleEntity.SaleItems.Add(new Entities.SaleItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = itemEntity.Price,
                    Sale = saleEntity,
                    Item = itemEntity
                });

                // Update inventory
                itemEntity.Quantity -= item.Quantity;
                itemRepository.Update(itemEntity);
            }
            
            var changeAmount = request.AmountPaid - totalAmount;
            
            saleEntity.ChangeAmount = changeAmount;
            saleEntity.AmountPaid = request.AmountPaid;

            // Save sale
            await saleRepository.AddAsync(saleEntity);
            await saleRepository.SaveChangesAsync();

            // Map to DTO
            var saleDto = mapper.Map<Sale>(saleEntity);
            saleDto.Change = CalculateChangeDenominations(changeAmount);;
            saleDto.AmountPaid = request.AmountPaid;
            
            return Result<Sale>.Success(saleDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating sale");
            return Result<Sale>.Failure("Failed to create sale");
        }
    }

    public async Task<Result<IEnumerable<Sale>>> GetAllSalesAsync()
    {
        try
        {
            var sales = await saleRepository.GetAllWithItemsAsync();
            var salesDto = mapper.Map<IEnumerable<Sale>>(sales);
            return Result<IEnumerable<Sale>>.Success(salesDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving sales");
            return Result<IEnumerable<Sale>>.Failure("Failed to retrieve sales");
        }
    }


    public async Task<Result<Sale>> GetSaleByIdAsync(Guid id)
    {
        try
        {
            // Use GetByIdWithItemsAsync instead of GetByIdAsync
            var sale = await saleRepository.GetByIdWithItemsAsync(id);
            if (sale == null)
            {
                return Result<Sale>.Failure($"Sale with ID {id} not found");
            }

            var saleDto = mapper.Map<Sale>(sale);
            return Result<Sale>.Success(saleDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving sale {SaleId}", id);
            return Result<Sale>.Failure("Failed to retrieve sale");
        }
    }


    public List<Change> CalculateChangeDenominations(decimal amount)
    {
        var result = new List<Change>();
        var remainingAmount = amount;

        foreach (var (value, name) in EuroDenominations.Denominations)
        {
            if (remainingAmount <= 0) break;

            var count = (int)(remainingAmount / value);
            if (count > 0)
            {
                result.Add(new Change
                {
                    Value = value,
                    Name = name,
                    Count = count
                });
                remainingAmount -= value * count;
            }
        }

        return result;
    }

    public async Task<Result<bool>> ValidateItemsAvailabilityAsync(IEnumerable<CreateSaleItem> items)
    {
        foreach (var item in items)
        {
            var itemEntity = await itemRepository.GetByIdAsync(item.ItemId);
            if (itemEntity == null)
            {
                return Result<bool>.Failure($"Item {item.ItemId} not found");
            }

            if (itemEntity.Quantity < item.Quantity)
            {
                return Result<bool>.Failure(
                    $"Insufficient stock for item {itemEntity.Name}. Available: {itemEntity.Quantity}, Requested: {item.Quantity}");
            }
        }

        return Result<bool>.Success(true);
    }

    public async Task<decimal> CalculateTotalAsync(IEnumerable<CreateSaleItem> items)
    {
        decimal total = 0;
        foreach (var item in items)
        {
            var itemEntity = await itemRepository.GetByIdAsync(item.ItemId);
            if (itemEntity != null)
            {
                total += itemEntity.Price * item.Quantity;
            }
        }
        return total;
    }
}