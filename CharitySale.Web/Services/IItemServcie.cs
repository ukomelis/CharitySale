using CharitySale.Shared.Models;

namespace CharitySale.Web.Services;

public interface IItemService
{
    Task<List<Item>> GetAllItemsAsync();
    Task<Item> GetItemByIdAsync(Guid id);
    Task<Item> UpdateItemQuantityAsync(Guid id, int quantity);
}