using AutoMapper;
using CharitySale.Api.Models;
using CharitySale.Api.Repositories;
using CharitySale.Shared.Models;
using Item = CharitySale.Api.Entities.Item;

namespace CharitySale.Api.Services;

public class ItemService(IItemRepository itemRepository, ILogger<ItemService> logger, IMapper mapper) : IItemService
{
    public async Task<Result<IEnumerable<Item>>> GetAllItemsAsync()
    {
        try
        {
            var items = await itemRepository.GetAllAsync();
            return Result<IEnumerable<Item>>.Success(items);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all items");
            return Result<IEnumerable<Item>>.Failure("Failed to retrieve items from database.");
        }
    }

    public async Task<Result<Item>> GetItemByIdAsync(Guid id)
    {
        try
        {
            var item = await itemRepository.GetByIdAsync(id);
            return item == null ? Result<Item>.Failure($"Item with ID {id} not found.") : Result<Item>.Success(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving item {Id}", id);
            return Result<Item>.Failure($"Failed to retrieve item {id} from database.");
        }
    }

    public async Task<Result<Item>> CreateItemAsync(CreateItem createItem)
    {
        try
        {
            var item = mapper.Map<Item>(createItem);
            await itemRepository.AddAsync(item);
            await itemRepository.SaveChangesAsync();

            return Result<Item>.Success(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating item {@CreateItem}", createItem);
            return Result<Item>.Failure("Failed to create item in database.");
        }
    }
    
    public async Task<Result<Item>> UpdateItemQuantityAsync(Guid id, int quantity)
    {
        return await UpdateItemQuantityInternalAsync(id, quantity, isAbsolute: true);
    }

    public async Task<Result<Item>> UpdateItemStockAsync(Guid id, int quantityChange)
    {
        return await UpdateItemQuantityInternalAsync(id, quantityChange, isAbsolute: false);
    }

    private async Task<Result<Item>> UpdateItemQuantityInternalAsync(Guid id, int quantityChange, bool isAbsolute)
    {
        try
        {
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return Result<Item>.Failure($"Item with ID {id} not found.");
            }

            var newQuantity = isAbsolute ? quantityChange : item.Quantity + quantityChange;
        
            if (newQuantity < 0)
            {
                return isAbsolute 
                    ? Result<Item>.Failure("Quantity cannot be negative.") 
                    : Result<Item>.Failure($"Cannot reduce quantity by {Math.Abs(quantityChange)} as only {item.Quantity} items are available.");
            }

            if (isAbsolute)
            {
                logger.LogInformation("Updating quantity for item {Id} from {OldQuantity} to {NewQuantity}", 
                    id, item.Quantity, newQuantity);
            }

            item.Quantity = newQuantity;
            await itemRepository.SaveChangesAsync();

            return Result<Item>.Success(item);
        }
        catch (Exception ex)
        {
            var message = isAbsolute 
                ? $"Error updating quantity for item {id} to {quantityChange}"
                : $"Error updating quantity for item {id} by {quantityChange}";
            
            logger.LogError(ex, message);
            return Result<Item>.Failure($"Failed to update quantity for item {id}.");
        }
    }


    public async Task<Result<bool>> DeleteItemAsync(Guid id)
    {
        try
        {
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return Result<bool>.Failure($"Item with ID {id} not found.");
            }

            itemRepository.Delete(item);
            await itemRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting item {Id}", id);
            return Result<bool>.Failure($"Failed to delete item {id} from database.");
        }
    }
}