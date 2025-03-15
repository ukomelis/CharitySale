namespace CharitySale.Api.Models.DTOs;

public class BaseSaleItemDto
{
    public int Quantity { get; set; }
}

public class CreateSaleItemDto : BaseSaleItemDto
{
    public Guid ItemId { get; set; }
}

public class SaleItemDto : BaseSaleItemDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}
