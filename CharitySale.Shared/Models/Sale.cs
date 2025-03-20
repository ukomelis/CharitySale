using System.ComponentModel.DataAnnotations;

namespace CharitySale.Shared.Models;

public class CreateSale
{
    [Required(ErrorMessage = "At least one item is required for a sale")]
    [MinLength(1, ErrorMessage = "At least one item is required for a sale")]
    public List<CreateSaleItem> Items { get; set; } = [];
    
    [Required(ErrorMessage = "Amount paid is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than 0")]
    public decimal AmountPaid { get; set; }
}

public class Sale
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }
    public List<SaleItem> Items { get; set; } = [];
    public List<Change> Change { get; set; } = [];

}