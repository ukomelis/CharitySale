namespace CharitySale.Shared.Models;

public class EuroDenominations
{
    public static readonly List<(decimal Value, string Name)> Denominations =
    [
        (500.00m, "€500"),
        (200.00m, "€200"),
        (100.00m, "€100"),
        (50.00m, "€50"),
        (20.00m, "€20"),
        (10.00m, "€10"),
        (5.00m, "€5"),

        (2.00m, "€2"),
        (1.00m, "€1"),
        (0.50m, "50c"),
        (0.20m, "20c"),
        (0.10m, "10c"),
        (0.05m, "5c"),
        (0.02m, "2c"),
        (0.01m, "1c")
    ];
}