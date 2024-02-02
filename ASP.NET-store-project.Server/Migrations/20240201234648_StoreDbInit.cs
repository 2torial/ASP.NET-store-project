using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ASP.NET_store_project.Server.Migrations
{
    /// <inheritdoc />
    public partial class StoreDbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdressDetails",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Region = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    StreetName = table.Column<string>(type: "text", nullable: false),
                    HouseNumber = table.Column<string>(type: "text", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdressDetails", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Type = table.Column<string>(type: "text", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "text", nullable: false),
                    Parameter = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "text", nullable: false),
                    PassWord = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDetails",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDetails", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Page = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_AdressDetails_OrderId",
                        column: x => x.OrderId,
                        principalTable: "AdressDetails",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_CustomerDetails_OrderId",
                        column: x => x.OrderId,
                        principalTable: "CustomerDetails",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemConfiguration",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    ConfigurationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemConfiguration", x => new { x.ConfigurationId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_ItemConfiguration_Configuration_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configuration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemConfiguration_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    StatusCode = table.Column<string>(type: "text", nullable: false),
                    DateOfChange = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => new { x.OrderId, x.StatusCode });
                    table.ForeignKey(
                        name: "FK_OrderStatus_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderStatus_Status_StatusCode",
                        column: x => x.StatusCode,
                        principalTable: "Status",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedItem_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.InsertData(
                table: "AdressDetails",
                columns: new[] { "OrderId", "ApartmentNumber", "City", "HouseNumber", "PostalCode", "Region", "StreetName" },
                values: new object[,]
                {
                    { 1, "91", "Bielsko-Biała", "17", "43-300", "Śląsk", "3 Maja" },
                    { 2, null, "Wrocław", "15", "50-383", "Dolny Śląsk", "Fryderyka Joliot-Curie" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Type", "Label" },
                values: new object[,]
                {
                    { "Headsets", "Headsets" },
                    { "Laptops", "Laptops/Notebooks/Ultrabooks" },
                    { "Microphones", "Microphones" }
                });

            migrationBuilder.InsertData(
                table: "Configuration",
                columns: new[] { "Id", "Label", "Order", "Parameter" },
                values: new object[,]
                {
                    { 1, "RAM Memory", 4, "4 GB" },
                    { 2, "RAM Memory", 8, "8 GB" },
                    { 3, "RAM Memory", 16, "16 GB" },
                    { 4, "RAM Memory", 32, "32 GB" },
                    { 5, "RAM Memory", 64, "64 GB" },
                    { 6, "RAM Memory", 9999, "No Memory" },
                    { 7, "System", 1, "Windows 10" },
                    { 8, "System", 2, "Windows 11" },
                    { 9, "System", 11, "MacOS" },
                    { 10, "System", 9999, "No System" },
                    { 11, "Disk", 1, "SSD" },
                    { 12, "Disk", 2, "SSD" },
                    { 13, "Disk", 9999, "No Disk" },
                    { 14, "Disk Capacity", 512, "512 GB" },
                    { 15, "Disk Capacity", 1024, "1024 GB" },
                    { 16, "Disk Capacity", 2048, "2048 GB" },
                    { 17, "Disk Capacity", 4096, "4096 GB" },
                    { 18, "Processor", 1, "Intel Core i3" },
                    { 19, "Processor", 1, "Intel Core i5" },
                    { 20, "Processor", 1, "Intel Core i7" },
                    { 21, "Processor", 1, "Intel Core i9" },
                    { 22, "Processor", 2, "Ryzen 3" },
                    { 23, "Processor", 2, "Ryzen 5" },
                    { 24, "Processor", 2, "Ryzen 7" },
                    { 25, "Processor", 2, "Ryzen 9" },
                    { 26, "Processor", 9999, "No Processor" },
                    { 27, "Cord Length", 1, "1 m" },
                    { 28, "Cord Length", 2, "2 m" }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "UserName", "IsAdmin", "PassWord" },
                values: new object[] { "root", true, "root" });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "UserName", "PassWord" },
                values: new object[] { "user", "user" });

            migrationBuilder.InsertData(
                table: "CustomerDetails",
                columns: new[] { "OrderId", "Email", "Name", "PhoneNumber", "Surname" },
                values: new object[,]
                {
                    { 1, "bartżur@tlen.o2", "Bartłomiej", "29 02 2024 0", "Żurowski" },
                    { 2, "stan3@rp.on", "Stanisław", "03 05 1791 0", "August" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                column: "Code",
                values: new object[]
                {
                    "Awaiting Delivery",
                    "Canceled",
                    "Delivered",
                    "Pending",
                    "Preparing",
                    "Returned",
                    "Sent"
                });

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "CategoryId", "Name", "Page", "Price" },
                values: new object[,]
                {
                    { 1, "Laptops", "Laptop #1", null, 900 },
                    { 2, "Laptops", "Laptop #2", null, 650 },
                    { 3, "Laptops", "Laptop #3", null, 800 },
                    { 4, "Laptops", "Laptop #4", null, 500 },
                    { 5, "Laptops", "Laptop #5", null, 660 },
                    { 6, "Laptops", "Laptop #6", null, 500 },
                    { 7, "Laptops", "Laptop #7", null, 450 },
                    { 8, "Headsets", "Headset #1", null, 100 },
                    { 9, "Headsets", "Headset #2", null, 300 },
                    { 10, "Headsets", "Headset #3", null, 50 },
                    { 11, "Microphones", "Microphone #1", null, 50 },
                    { 12, "Microphones", "Microphone #2", null, 20 }
                });

            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "OrderId", "CustomerId" },
                values: new object[,]
                {
                    { 1, "user" },
                    { 2, "root" }
                });

            migrationBuilder.InsertData(
                table: "Image",
                columns: new[] { "Id", "Content", "ItemId" },
                values: new object[,]
                {
                    { 1, "https://placehold.co/150x150", 1 },
                    { 2, "https://placehold.co/150x150", 2 },
                    { 3, "https://placehold.co/150x150", 2 },
                    { 4, "https://placehold.co/150x150", 2 },
                    { 5, "https://placehold.co/150x150", 3 },
                    { 6, "https://placehold.co/150x150", 4 },
                    { 7, "https://placehold.co/150x150", 5 },
                    { 8, "https://placehold.co/150x150", 6 },
                    { 9, "https://placehold.co/150x150", 7 },
                    { 10, "https://placehold.co/150x150", 8 },
                    { 11, "https://placehold.co/150x150", 9 },
                    { 12, "https://placehold.co/150x150", 9 },
                    { 13, "https://placehold.co/150x150", 10 },
                    { 14, "https://placehold.co/150x150", 11 },
                    { 15, "https://placehold.co/150x150", 12 }
                });

            migrationBuilder.InsertData(
                table: "ItemConfiguration",
                columns: new[] { "ConfigurationId", "ItemId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 3, 2 },
                    { 3, 4 },
                    { 5, 1 },
                    { 5, 5 },
                    { 6, 3 },
                    { 7, 2 },
                    { 8, 1 },
                    { 8, 4 },
                    { 9, 5 },
                    { 10, 1 },
                    { 10, 6 },
                    { 11, 2 },
                    { 11, 7 },
                    { 14, 2 },
                    { 15, 6 },
                    { 15, 7 },
                    { 16, 1 },
                    { 19, 1 },
                    { 21, 2 },
                    { 22, 7 }
                });

            migrationBuilder.InsertData(
                table: "SelectedItem",
                columns: new[] { "Id", "CustomerId", "ItemId", "OrderId", "Quantity" },
                values: new object[,]
                {
                    { 1, "user", 1, 1, 1 },
                    { 2, "user", 8, 1, 1 },
                    { 3, "user", 12, 1, 1 },
                    { 4, "root", 4, 2, 10 },
                    { 5, "root", 1, 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_ItemId",
                table: "Image",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryId",
                table: "Item",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemConfiguration_ItemId",
                table: "ItemConfiguration",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatus_StatusCode",
                table: "OrderStatus",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedItem_CustomerId",
                table: "SelectedItem",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedItem_ItemId",
                table: "SelectedItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedItem_OrderId",
                table: "SelectedItem",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "ItemConfiguration");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "SelectedItem");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "AdressDetails");

            migrationBuilder.DropTable(
                name: "CustomerDetails");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
