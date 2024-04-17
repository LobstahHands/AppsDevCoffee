﻿// <auto-generated />
using System;
using AppsDevCoffee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppsDevCoffee.Migrations
{
    [DbContext(typeof(CoffeeAppContext))]
    partial class CoffeeAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AppsDevCoffee.Models.AccountLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogResult")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AccountLogs");
                });

            modelBuilder.Entity("AppsDevCoffee.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PaidDate")
                        .HasColumnType("datetime2");

                    b.Property<float?>("PriceAdjustment")
                        .HasColumnType("real");

                    b.Property<float?>("SubtotalCost")
                        .HasColumnType("real");

                    b.Property<float?>("TotalCost")
                        .HasColumnType("real");

                    b.Property<float?>("TotalPaid")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrderDate = new DateTime(2024, 4, 16, 21, 53, 44, 49, DateTimeKind.Local).AddTicks(4515),
                            OrderStatus = "Filled",
                            PriceAdjustment = 0f,
                            SubtotalCost = 15f,
                            TotalCost = 15f,
                            TotalPaid = 15f,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            OrderDate = new DateTime(2024, 4, 16, 21, 53, 44, 49, DateTimeKind.Local).AddTicks(4525),
                            OrderStatus = "Pending",
                            PriceAdjustment = 0f,
                            SubtotalCost = 25f,
                            TotalCost = 25f,
                            TotalPaid = 0f,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("AppsDevCoffee.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemId"));

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("OriginTypeId")
                        .HasColumnType("int");

                    b.Property<float>("OzQuantity")
                        .HasColumnType("real");

                    b.Property<int>("RoastTypeId")
                        .HasColumnType("int");

                    b.Property<float>("Subtotal")
                        .HasColumnType("real");

                    b.HasKey("OrderItemId");

                    b.HasIndex("OrderId");

                    b.HasIndex("OriginTypeId");

                    b.HasIndex("RoastTypeId");

                    b.ToTable("OrderItems");

                    b.HasData(
                        new
                        {
                            OrderItemId = 1,
                            OrderId = 1,
                            OriginTypeId = 1,
                            OzQuantity = 15f,
                            RoastTypeId = 1,
                            Subtotal = 15f
                        });
                });

            modelBuilder.Entity("AppsDevCoffee.Models.OriginType", b =>
                {
                    b.Property<int>("OriginTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OriginTypeId"));

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<float>("CostPerOz")
                        .HasColumnType("real");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoasterNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierNotes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OriginTypeId");

                    b.ToTable("OriginTypes");

                    b.HasData(
                        new
                        {
                            OriginTypeId = 1,
                            Active = 1,
                            CostPerOz = 1f,
                            Country = "Colombia",
                            RoasterNotes = "Bold at medium, rich at dark",
                            SupplierNotes = "Versatile"
                        },
                        new
                        {
                            OriginTypeId = 2,
                            Active = 1,
                            CostPerOz = 1f,
                            Country = "Costa Rica",
                            RoasterNotes = "Fruity and acidic at light, rounds out to a bold at medium",
                            SupplierNotes = "Bright and Punch"
                        },
                        new
                        {
                            OriginTypeId = 3,
                            Active = 1,
                            CostPerOz = 1f,
                            Country = "Mexico",
                            RoasterNotes = "Bold at medium, rich at dark",
                            SupplierNotes = "Decaf - floral and honey tasting notes"
                        });
                });

            modelBuilder.Entity("AppsDevCoffee.Models.RoastType", b =>
                {
                    b.Property<int>("RoastTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoastTypeId"));

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoastTypeId");

                    b.ToTable("RoastTypes");

                    b.HasData(
                        new
                        {
                            RoastTypeId = 1,
                            Active = 1,
                            Description = "Light"
                        },
                        new
                        {
                            RoastTypeId = 2,
                            Active = 1,
                            Description = "Medium"
                        },
                        new
                        {
                            RoastTypeId = 3,
                            Active = 1,
                            Description = "Dark"
                        });
                });

            modelBuilder.Entity("AppsDevCoffee.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hashed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserTypeId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateAdded = new DateTime(2024, 4, 16, 21, 53, 44, 49, DateTimeKind.Local).AddTicks(4226),
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            Hashed = "PassPass1!",
                            LastName = "User",
                            UserStatus = "Active",
                            UserTypeId = 1,
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            DateAdded = new DateTime(2024, 4, 16, 21, 53, 44, 49, DateTimeKind.Local).AddTicks(4304),
                            Email = "john@example.com",
                            FirstName = "JohnTest",
                            Hashed = "PassPass1!",
                            LastName = "Doe",
                            UserStatus = "Active",
                            UserTypeId = 2,
                            Username = "john"
                        },
                        new
                        {
                            Id = 3,
                            DateAdded = new DateTime(2024, 4, 16, 21, 53, 44, 49, DateTimeKind.Local).AddTicks(4309),
                            Email = "jane@example.com",
                            FirstName = "JaneTest",
                            Hashed = "PassPass1!",
                            LastName = "Doe",
                            UserStatus = "Active",
                            UserTypeId = 3,
                            Username = "jane"
                        });
                });

            modelBuilder.Entity("AppsDevCoffee.Models.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Active")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = 0,
                            DateAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Active = 0,
                            DateAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Employee"
                        },
                        new
                        {
                            Id = 3,
                            Active = 0,
                            DateAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "User"
                        });
                });

            modelBuilder.Entity("AppsDevCoffee.Models.Order", b =>
                {
                    b.HasOne("AppsDevCoffee.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AppsDevCoffee.Models.OrderItem", b =>
                {
                    b.HasOne("AppsDevCoffee.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId");

                    b.HasOne("AppsDevCoffee.Models.OriginType", "OriginType")
                        .WithMany()
                        .HasForeignKey("OriginTypeId");

                    b.HasOne("AppsDevCoffee.Models.RoastType", "RoastType")
                        .WithMany()
                        .HasForeignKey("RoastTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("OriginType");

                    b.Navigation("RoastType");
                });

            modelBuilder.Entity("AppsDevCoffee.Models.User", b =>
                {
                    b.HasOne("AppsDevCoffee.Models.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("AppsDevCoffee.Models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("AppsDevCoffee.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
