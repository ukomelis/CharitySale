using System.Collections.Generic;

namespace CharitySale.Api.Entities;

public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public Category Category { get; set; }

    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}

public enum Category
{
    Food = 0,
    Other = 1
}