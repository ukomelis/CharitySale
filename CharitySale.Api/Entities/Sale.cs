namespace CharitySale.Api.Entities;

public class Sale
{
    public Guid Id { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }
    
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
