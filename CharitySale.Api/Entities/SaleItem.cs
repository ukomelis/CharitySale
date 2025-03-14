namespace CharitySale.Api.Entities;

public class SaleItem
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Price at time of sale
    
    public Sale Sale { get; set; } = null!;
    public Item Item { get; set; } = null!;
}
