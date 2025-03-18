namespace CharitySale.Api.Entities;

public class Item
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Category Category { get; set; }
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}