using System.ComponentModel.DataAnnotations;

namespace CharitySale.Shared.Models;

public class CreateSaleItem
{
    [Required(ErrorMessage = "Item ID is required")]
    public Guid ItemId { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
    public int Quantity { get; set; }
}

public class SaleItem
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}