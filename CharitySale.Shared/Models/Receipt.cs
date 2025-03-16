namespace CharitySale.Shared.Models;

public class Receipt
{
    public Guid SaleId { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }
    public List<SaleItem> Items { get; set; } = [];
    public List<Change> Change { get; set; } = [];
}