using CharitySale.Shared.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CharitySale.Shared.Models;

public abstract class BaseItem
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Category Category { get; set; }
}

public class Item : BaseItem
{
    public Guid Id { get; set; }
}

public class CreateItem : BaseItem
{
}

public class UpdateItemQuantity
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be between 0 and 1000")]
    public int Quantity { get; set; }
}
