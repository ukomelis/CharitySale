using System.ComponentModel.DataAnnotations;

namespace CharitySale.Shared.Models;

public class BaseSale
{
    public DateTime SaleDate { get; set; }
}

public class CreateSale : BaseSale
{
    [Required(ErrorMessage = "At least one item is required for a sale")]
    [MinLength(1, ErrorMessage = "At least one item is required for a sale")]
    public List<CreateSaleItem> Items { get; set; } = [];
    
    [Required(ErrorMessage = "Amount paid is required")]
    [Range(0.01, 100000, ErrorMessage = "Amount paid must be greater than 0")]
    public decimal AmountPaid { get; set; }
}

public class Sale : BaseSale
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItem> Items { get; set; } = [];
    public List<Change> Change { get; set; } = [];
}