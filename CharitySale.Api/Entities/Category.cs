namespace CharitySale.Api.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<Item> Items { get; set; } = [];
}