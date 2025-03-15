using CharitySale.Api.Entities;
using CharitySale.Api.Models;
using CharitySale.Api.Models.DTOs;
using CharitySale.Api.Repositories;

namespace CharitySale.Api.Services;

public class ItemService(IItemRepository itemRepository) : IItemService
{
    public async Task<Result<IEnumerable<Item>>> GetAllItemsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Item>> GetItemByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Item>> CreateItemAsync(CreateItemDto itemDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Item>> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Item>> UpdateItemQuantityAsync(Guid id, int quantityChange)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}