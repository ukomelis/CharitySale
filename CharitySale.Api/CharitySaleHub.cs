using Microsoft.AspNetCore.SignalR;

namespace CharitySale.Api;

public class CharitySaleHub : Hub
{
    public async Task UpdateItemQuantity(Guid itemId, int newQuantity)
    {
        await Clients.All.SendAsync("ItemQuantityUpdated", itemId, newQuantity);
    }

    public async Task NotifySaleCreated(Guid saleId)
    {
        await Clients.All.SendAsync("SaleCreated", saleId);
    }
}