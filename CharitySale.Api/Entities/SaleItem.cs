using System.Text.Json.Serialization;

namespace CharitySale.Api.Entities;

public class SaleItem
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Price at time of sale
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public required Sale Sale { get; set; }
    [JsonIgnore]
    public required Item Item { get; set; }
}
