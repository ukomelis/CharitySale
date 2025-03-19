using System.Net.Http.Json;
using CharitySale.Shared.Models;

namespace CharitySale.Web.Services;

public class SaleService(IHttpClientFactory httpClientFactory) : ISaleService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("CharitySaleApi");

    public async Task<Sale> CreateSaleAsync(CreateSale sale)
    {
        var response = await _httpClient.PostAsJsonAsync("api/sales", sale);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Sale>() 
               ?? throw new Exception("Failed to create sale");
    }

    public async Task<List<Sale>> GetAllSalesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Sale>>("api/sales");
        return response ?? new List<Sale>();
    }

    public async Task<Sale> GetSaleByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Sale>($"api/sales/{id}")
               ?? throw new Exception($"Sale with ID {id} not found");
    }
}