namespace CharitySale.Api.Models.DTOs;

public abstract class BaseSaleDto
{
    public DateTime SaleDate { get; set; }
}

public class CreateSaleDto : BaseSaleDto
{
    public List<CreateSaleItemDto> Items { get; set; } = [];
}

public class SaleDto : BaseSaleDto
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemDto> Items { get; set; } = [];
}
