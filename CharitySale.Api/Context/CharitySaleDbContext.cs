using CharitySale.Api.Entities;
using CharitySale.Api.Models.Enums;
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
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Price).IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(10);
            entity.Property(e => e.ImageUrl).HasMaxLength(100);
            
            // Add check constraint for non-negative quantity
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Quantity_NonNegative", "[Quantity] >= 0"));
        
            // Add check constraint for positive price
            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Price_Positive", "[Price] >= 0"));
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.SaleDate).IsRequired();
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            
            // Add check constraint for non-negative total amount
            entity.ToTable(t => t.HasCheckConstraint("CK_Sale_TotalAmount_NonNegative", "[TotalAmount] >= 0"));

        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            
            // Add check constraints
            entity.ToTable(t => t.HasCheckConstraint("CK_SaleItem_Quantity_Positive", "[Quantity] > 0"));
            entity.ToTable(t => t.HasCheckConstraint("CK_SaleItem_UnitPrice_NonNegative", "[UnitPrice] >= 0"));


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

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().HasData(
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e9f"),
                Name = "Brownie", 
                Price = 0.65m, 
                Quantity = 48, 
                Category = Category.Food, 
                ImageUrl = "/images/brownie.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e10"),
                Name = "Muffin", 
                Price = 1.00m, 
                Quantity = 36, 
                Category = Category.Food, 
                ImageUrl = "/images/muffin.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e11"),
                Name = "Cake Pop", 
                Price = 1.35m, 
                Quantity = 24, 
                Category = Category.Food, 
                ImageUrl = "/images/cakepop.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e12"),
                Name = "Apple tart", 
                Price = 1.50m, 
                Quantity = 60, 
                Category = Category.Food, 
                ImageUrl = "/images/appletart.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e13"),
                Name = "Water", 
                Price = 1.50m, 
                Quantity = 30, 
                Category = Category.Food, 
                ImageUrl = "/images/water.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e14"),
                Name = "Shirt", 
                Price = 2.00m, 
                Quantity = 0, 
                Category = Category.Other, 
                ImageUrl = "/images/shirt.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e15"),
                Name = "Pants", 
                Price = 3.00m, 
                Quantity = 0, 
                Category = Category.Other, 
                ImageUrl = "/images/pants.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e16"),
                Name = "Jacket", 
                Price = 4.00m, 
                Quantity = 0, 
                Category = Category.Other, 
                ImageUrl = "/images/jacket.jpg" 
            },
            new Item 
            { 
                Id = Guid.Parse("81a130d2-502f-4cf1-a376-63edeb000e17"),
                Name = "Toy", 
                Price = 1.00m, 
                Quantity = 0, 
                Category = Category.Other, 
                ImageUrl = "/images/toy.jpg" 
            }
        );
    }
}