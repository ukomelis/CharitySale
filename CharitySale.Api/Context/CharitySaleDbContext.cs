using CharitySale.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CharitySale.Api.Context;

public class CharitySaleDbContext(DbContextOptions<CharitySaleDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(10);
            entity.Property(e => e.ImageUrl).HasMaxLength(100);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SaleDate).IsRequired();
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            entity.HasOne(si => si.Sale)
                .WithMany(s => s.SaleItems)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(si => si.Item)
                .WithMany(i => i.SaleItems)
                .HasForeignKey(si => si.ItemId)
                .OnDelete(DeleteBehavior.Restrict); // Use Restrict to prevent deletion of items with sales
        });

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Brownie", Price = 0.65m, Quantity = 48, Category = Category.Food, ImageUrl = "/images/brownie.jpg" },
            new Item { Id = 2, Name = "Muffin", Price = 1.00m, Quantity = 36, Category = Category.Food, ImageUrl = "/images/muffin.jpg" },
            new Item { Id = 3, Name = "Cake Pop", Price = 1.35m, Quantity = 24, Category = Category.Food, ImageUrl = "/images/cakepop.jpg" },
            new Item { Id = 4, Name = "Apple tart", Price = 1.50m, Quantity = 60, Category = Category.Food, ImageUrl = "/images/appletart.jpg" },
            new Item { Id = 5, Name = "Water", Price = 1.50m, Quantity = 30, Category = Category.Food, ImageUrl = "/images/water.jpg" },
            
            new Item { Id = 6, Name = "Shirt", Price = 2.00m, Quantity = 0, Category = Category.Other, ImageUrl = "/images/shirt.jpg" },
            new Item { Id = 7, Name = "Pants", Price = 3.00m, Quantity = 0, Category = Category.Other, ImageUrl = "/images/pants.jpg" },
            new Item { Id = 8, Name = "Jacket", Price = 4.00m, Quantity = 0, Category = Category.Other, ImageUrl = "/images/jacket.jpg" },
            new Item { Id = 9, Name = "Toy", Price = 1.00m, Quantity = 0, Category = Category.Other, ImageUrl = "/images/toy.jpg" }
        );
    }
}