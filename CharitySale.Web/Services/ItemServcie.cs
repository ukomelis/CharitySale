using System.Net.Http.Json;
using CharitySale.Shared.Models;

namespace CharitySale.Web.Services;

public class ItemService(IHttpClientFactory httpClientFactory) : IItemService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CharitySaleApi");

    public async Task<List<Item>> GetAllItemsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Item>>("api/items");
        return response ?? [];
    }

    public async Task<Item> GetItemByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Item>($"api/items/{id}") 
               ?? throw new Exception($"Item with ID {id} not found");
    }

    public async Task<Item> UpdateItemQuantityAsync(Guid id, int quantity)
    {
        var request = new UpdateItemQuantity { Quantity = quantity };
        var response = await _httpClient.PatchAsJsonAsync($"api/items/{id}/quantity", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Item>() 
               ?? throw new Exception("Failed to update item quantity");
    }
}