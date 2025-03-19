﻿// <auto-generated />
using System;
using CharitySale.Api.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CharitySale.Api.Migrations
{
    [DbContext(typeof(CharitySaleDbContext))]
    partial class CharitySaleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CharitySale.Api.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Food and beverages"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Clothing"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Other"
                        });
                });

            modelBuilder.Entity("CharitySale.Api.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Items", t =>
                        {
                            t.HasCheckConstraint("CK_Item_Price_Positive", "\"Price\" >= 0");

                            t.HasCheckConstraint("CK_Item_Quantity_NonNegative", "\"Quantity\" >= 0");
                        });

                    b.HasData(
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e9f"),
                            CategoryId = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/brownie.png",
                            Name = "Brownie",
                            Price = 0.65m,
                            Quantity = 48,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e10"),
                            CategoryId = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/muffin.png",
                            Name = "Muffin",
                            Price = 1.00m,
                            Quantity = 36,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e11"),
                            CategoryId = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/cake-pop.png",
                            Name = "Cake Pop",
                            Price = 1.35m,
                            Quantity = 24,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e12"),
                            CategoryId = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/pie-cake.png",
                            Name = "Apple tart",
                            Price = 1.50m,
                            Quantity = 60,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e13"),
                            CategoryId = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/water.png",
                            Name = "Water",
                            Price = 1.50m,
                            Quantity = 30,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e14"),
                            CategoryId = 2,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/tshirt.png",
                            Name = "Shirt",
                            Price = 2.00m,
                            Quantity = 0,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e15"),
                            CategoryId = 2,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/pants.png",
                            Name = "Pants",
                            Price = 3.00m,
                            Quantity = 0,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e16"),
                            CategoryId = 2,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/jacket.png",
                            Name = "Jacket",
                            Price = 4.00m,
                            Quantity = 0,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("81a130d2-502f-4cf1-a376-63edeb000e17"),
                            CategoryId = 3,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageUrl = "/images/toys.png",
                            Name = "Toy",
                            Price = 1.00m,
                            Quantity = 0,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("CharitySale.Api.Entities.Sale", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("numeric");

                    b.Property<decimal>("ChangeAmount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.ToTable("Sales", t =>
                        {
                            t.HasCheckConstraint("CK_Sale_TotalAmount_NonNegative", "\"TotalAmount\" >= 0");
                        });
                });

            modelBuilder.Entity("CharitySale.Api.Entities.SaleItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SaleId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleItems", t =>
                        {
                            t.HasCheckConstraint("CK_SaleItem_Quantity_Positive", "\"Quantity\" > 0");

                            t.HasCheckConstraint("CK_SaleItem_UnitPrice_NonNegative", "\"UnitPrice\" >= 0");
                        });
                });

            modelBuilder.Entity("CharitySale.Api.Entities.Item", b =>
                {
                    b.HasOne("CharitySale.Api.Entities.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("CharitySale.Api.Entities.SaleItem", b =>
                {
                    b.HasOne("CharitySale.Api.Entities.Item", "Item")
                        .WithMany("SaleItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CharitySale.Api.Entities.Sale", "Sale")
                        .WithMany("SaleItems")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("CharitySale.Api.Entities.Category", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("CharitySale.Api.Entities.Item", b =>
                {
                    b.Navigation("SaleItems");
                });

            modelBuilder.Entity("CharitySale.Api.Entities.Sale", b =>
                {
                    b.Navigation("SaleItems");
                });
#pragma warning restore 612, 618
        }
    }
}
