using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppsDevCoffee.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OriginTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoasterNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoastTypeId = table.Column<int>(type: "int", nullable: false),
                    TotalRoastTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FirstCrackTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SecondCrackTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CoolAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    GreenWeightOz = table.Column<float>(type: "real", nullable: false),
                    RoastedWeightOz = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoastTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoastTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OzQuantity = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoastId = table.Column<int>(type: "int", nullable: false),
                    PerOzPrice = table.Column<float>(type: "real", nullable: false),
                    OriginTypeId = table.Column<int>(type: "int", nullable: false),
                    TierTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrentInventories_OriginTypes_OriginTypeId",
                        column: x => x.OriginTypeId,
                        principalTable: "OriginTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrentInventories_Roasts_RoastId",
                        column: x => x.RoastId,
                        principalTable: "Roasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OzQuantity = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoastId = table.Column<int>(type: "int", nullable: false),
                    PerOzPrice = table.Column<float>(type: "real", nullable: false),
                    OriginTypeId = table.Column<int>(type: "int", nullable: false),
                    TierTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryLogs_OriginTypes_OriginTypeId",
                        column: x => x.OriginTypeId,
                        principalTable: "OriginTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryLogs_Roasts_RoastId",
                        column: x => x.RoastId,
                        principalTable: "Roasts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalPaid = table.Column<float>(type: "real", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubtotalCost = table.Column<float>(type: "real", nullable: false),
                    PriceAdjustment = table.Column<float>(type: "real", nullable: false),
                    TotalCost = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CurrentInventoryId = table.Column<int>(type: "int", nullable: false),
                    OzQuantity = table.Column<float>(type: "real", nullable: false),
                    CostPerOz = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_CurrentInventories_CurrentInventoryId",
                        column: x => x.CurrentInventoryId,
                        principalTable: "CurrentInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "Id", "Active", "DateAdded", "Description" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                    { 2, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" },
                    { 3, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateAdded", "Email", "FirstName", "Hashed", "LastName", "UserStatus", "UserTypeId", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 10, 8, 26, 17, 531, DateTimeKind.Local).AddTicks(6838), "admin@example.com", "Admin", "PassPass1!", "User", "Active", 1, "admin" },
                    { 2, new DateTime(2024, 4, 10, 8, 26, 17, 531, DateTimeKind.Local).AddTicks(6886), "john@example.com", "JohnTest", "PassPass1!", "Doe", "Active", 2, "john" },
                    { 3, new DateTime(2024, 4, 10, 8, 26, 17, 531, DateTimeKind.Local).AddTicks(6889), "jane@example.com", "JaneTest", "PassPass1!", "Doe", "Active", 3, "jane" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentInventories_OriginTypeId",
                table: "CurrentInventories",
                column: "OriginTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentInventories_RoastId",
                table: "CurrentInventories",
                column: "RoastId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLogs_OriginTypeId",
                table: "InventoryLogs",
                column: "OriginTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLogs_RoastId",
                table: "InventoryLogs",
                column: "RoastId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CurrentInventoryId",
                table: "OrderItems",
                column: "CurrentInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryLogs");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "RoastTypes");

            migrationBuilder.DropTable(
                name: "CurrentInventories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OriginTypes");

            migrationBuilder.DropTable(
                name: "Roasts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
