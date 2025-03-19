using CharitySale.Shared.Models;

namespace CharitySale.Web.Models;

public class Cart
{
    private readonly Dictionary<Guid, CartItem> _items = new();

    public IReadOnlyCollection<CartItem> Items => _items.Values;
    
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    
    public int TotalQuantity => Items.Sum(item => item.Quantity);
    
    public void AddItem(Item item)
    {
        if (_items.TryGetValue(item.Id, out var cartItem))
        {
            cartItem.Quantity++;
        }
        else
        {
            _items[item.Id] = new CartItem
            {
                ItemId = item.Id,
                Name = item.Name ?? string.Empty,
                Price = item.Price,
                Quantity = 1,
                ImageUrl = item.ImageUrl ?? string.Empty
            };
        }
    }
    
    public void RemoveItem(Guid itemId)
    {
        if (_items.TryGetValue(itemId, out var cartItem))
        {
            cartItem.Quantity--;
            if (cartItem.Quantity <= 0)
            {
                _items.Remove(itemId);
            }
        }
    }
    
    public void Clear()
    {
        _items.Clear();
    }
    
    public CreateSale ToCreateSale(decimal amountPaid)
    {
        return new CreateSale
        {
            Items = _items.Values.Select(item => new CreateSaleItem
            {
                ItemId = item.ItemId,
                ItemName = item.Name,
                Quantity = item.Quantity
            }).ToList(),
            AmountPaid = amountPaid
        };
    }
}