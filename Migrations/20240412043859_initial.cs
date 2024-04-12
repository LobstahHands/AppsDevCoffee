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
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OriginTypes",
                columns: table => new
                {
                    OriginTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoasterNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostPerOz = table.Column<float>(type: "real", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginTypes", x => x.OriginTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RoastTypes",
                columns: table => new
                {
                    RoastTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoastTypes", x => x.RoastTypeId);
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
                    TotalPaid = table.Column<float>(type: "real", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubtotalCost = table.Column<float>(type: "real", nullable: true),
                    PriceAdjustment = table.Column<float>(type: "real", nullable: true),
                    TotalCost = table.Column<float>(type: "real", nullable: true)
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
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OriginTypeId = table.Column<int>(type: "int", nullable: false),
                    RoastTypeId = table.Column<int>(type: "int", nullable: false),
                    OzQuantity = table.Column<float>(type: "real", nullable: false),
                    Subtotal = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_OriginTypes_OriginTypeId",
                        column: x => x.OriginTypeId,
                        principalTable: "OriginTypes",
                        principalColumn: "OriginTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_RoastTypes_RoastTypeId",
                        column: x => x.RoastTypeId,
                        principalTable: "RoastTypes",
                        principalColumn: "RoastTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OriginTypes",
                columns: new[] { "OriginTypeId", "Active", "CostPerOz", "Country", "RoasterNotes", "SupplierNotes" },
                values: new object[,]
                {
                    { 1, 1, 1f, "Colombia", "Bold at medium, rich at dark", "Versatile" },
                    { 2, 1, 1f, "Costa Rica", "Fruity and acidic at light, rounds out to a bold at medium", "Bright and Punch" },
                    { 3, 1, 1f, "Mexico", "Bold at medium, rich at dark", "Decaf - floral and honey tasting notes" }
                });

            migrationBuilder.InsertData(
                table: "RoastTypes",
                columns: new[] { "RoastTypeId", "Active", "Description" },
                values: new object[,]
                {
                    { 1, 1, "Light" },
                    { 2, 1, "Medium" },
                    { 3, 1, "Dark" }
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
                    { 1, new DateTime(2024, 4, 11, 23, 38, 58, 255, DateTimeKind.Local).AddTicks(7970), "admin@example.com", "Admin", "PassPass1!", "User", "Active", 1, "admin" },
                    { 2, new DateTime(2024, 4, 11, 23, 38, 58, 255, DateTimeKind.Local).AddTicks(8020), "john@example.com", "JohnTest", "PassPass1!", "Doe", "Active", 2, "john" },
                    { 3, new DateTime(2024, 4, 11, 23, 38, 58, 255, DateTimeKind.Local).AddTicks(8024), "jane@example.com", "JaneTest", "PassPass1!", "Doe", "Active", 3, "jane" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "PaidDate", "PriceAdjustment", "SubtotalCost", "TotalCost", "TotalPaid", "UserId" },
                values: new object[] { 1, new DateTime(2024, 4, 11, 23, 38, 58, 255, DateTimeKind.Local).AddTicks(8122), null, 0f, 15f, 15f, 15f, 1 });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "OriginTypeId", "OzQuantity", "RoastTypeId", "Subtotal" },
                values: new object[] { 1, 1, 1, 15f, 1, 15f });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OriginTypeId",
                table: "OrderItems",
                column: "OriginTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RoastTypeId",
                table: "OrderItems",
                column: "RoastTypeId");

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
                name: "Logs");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OriginTypes");

            migrationBuilder.DropTable(
                name: "RoastTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
