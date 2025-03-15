namespace CharitySale.Api.Entities;

public class SaleItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Price at time of sale
    
    public required Sale Sale { get; set; }
    public required Item Item { get; set; }
}
