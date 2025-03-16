using CharitySale.Api.Models;
using CharitySale.Shared.Models;
using Item = CharitySale.Api.Entities.Item;

namespace CharitySale.Api.Services;

public interface IItemService
{
    Task<Result<IEnumerable<Item>>> GetAllItemsAsync();
    Task<Result<Item>> GetItemByIdAsync(Guid id);
    Task<Result<Item>> CreateItemAsync(CreateItem item);
    Task<Result<Item>> UpdateItemQuantityAsync(Guid id, int quantity);
    Task<Result<Item>> UpdateItemStockAsync(Guid id, int quantityChange);
    Task<Result<bool>> DeleteItemAsync(Guid id);
}