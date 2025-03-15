using CharitySale.Api.Entities;
using CharitySale.Api.Models;
using CharitySale.Api.Models.DTOs;

namespace CharitySale.Api.Services;

public interface IItemService
{
    Task<Result<IEnumerable<Item>>> GetAllItemsAsync();
    Task<Result<Item>> GetItemByIdAsync(Guid id);
    Task<Result<Item>> CreateItemAsync(CreateItemDto itemDto);
    Task<Result<Item>> UpdateItemAsync(Guid id, UpdateItemDto itemDto);
    Task<Result<Item>> UpdateItemQuantityAsync(Guid id, int quantityChange);
    Task<Result<bool>> DeleteItemAsync(Guid id);
}