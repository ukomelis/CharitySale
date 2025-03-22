using AutoMapper;
using CharitySale.Api.Models;
using CharitySale.Api.Repositories;
using CharitySale.Shared.Models;

namespace CharitySale.Api.Services;

public class ItemService(IItemRepository itemRepository, ILogger<ItemService> logger, IMapper mapper) : IItemService
{
    public async Task<Result<IEnumerable<Item>>> GetAllItemsAsync()
    {
        try
        {
            var items = await itemRepository.GetAllAsync();
            var itemDtos = mapper.Map<IEnumerable<Item>>(items);
            return Result<IEnumerable<Item>>.Success(itemDtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving items");
            return Result<IEnumerable<Item>>.Failure("Failed to retrieve items");
        }
    }

    public async Task<Result<Item>> GetItemByIdAsync(Guid id)
    {
        try
        {
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return Result<Item>.Failure($"Item with ID {id} not found");
            }

            var itemDto = mapper.Map<Item>(item);
            return Result<Item>.Success(itemDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving item {ItemId}", id);
            return Result<Item>.Failure("Failed to retrieve item");
        }
    }

    public async Task<Result<Item>> CreateItemAsync(CreateItem createItem)
    {
        try
        {
            var item = mapper.Map<Entities.Item>(createItem);
            
            await itemRepository.AddAsync(item);
            var saved = await itemRepository.SaveChangesAsync();

            if (!saved)
            {
                return Result<Item>.Failure("Failed to save the item");
            }

            var itemDto = mapper.Map<Item>(item);
            return Result<Item>.Success(itemDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating item");
            return Result<Item>.Failure("Failed to create item");
        }
    }

    public async Task<Result<Item>> SetItemStockAsync(Guid id, int quantity)
    {
        return await UpdateItemQuantityInternalAsync(id, quantity, true);
    }

    public async Task<Result<Item>> DecrementItemStockAsync(Guid id, int quantityChange)
    {
        return await UpdateItemQuantityInternalAsync(id, quantityChange, false);
    }

    public async Task<Result<Item>> UpdateItemQuantityInternalAsync(Guid id, int quantityChange, bool isAbsolute)
    {
        try
        {
            var item = await itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return Result<Item>.Failure($"Item with ID {id} not found");
            }

            if (isAbsolute)
            {
                item.Quantity = quantityChange;
            }
            else
            {
                var newQuantity = item.Quantity + quantityChange;
                if (newQuantity < 0)
                {
                    return Result<Item>.Failure($"Cannot reduce quantity by {Math.Abs(quantityChange)}. Only {item.Quantity} items available.");
                }
                item.Quantity = newQuantity;
            }
            
            item.UpdatedAt = DateTime.UtcNow;
            itemRepository.Update(item);
            var saved = await itemRepository.SaveChangesAsync();

            if (!saved)
            {
                return Result<Item>.Failure("Failed to update item quantity");
            }

            var itemDto = mapper.Map<Item>(item);
            return Result<Item>.Success(itemDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating item {ItemId} quantity", id);
            return Result<Item>.Failure("Failed to update item quantity");
        }
    }
}