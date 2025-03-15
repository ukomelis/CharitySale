using CharitySale.Api.Models.Enums;

namespace CharitySale.Api.Models.DTOs;

public abstract class BaseItemDto
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public Category Category { get; set; }
}

public class ItemDto : BaseItemDto
{
    public Guid Id { get; set; }
}

public class CreateItemDto : BaseItemDto
{
}

public class UpdateItemDto : BaseItemDto
{
}


