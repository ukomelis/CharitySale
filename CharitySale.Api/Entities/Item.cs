using CharitySale.Shared.Models.Enums;

namespace CharitySale.Api.Entities;

public class Item
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public Category Category { get; set; }

    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}