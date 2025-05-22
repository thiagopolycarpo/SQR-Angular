using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SQRBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialCode = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialDescription = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialCode);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductCode = table.Column<string>(type: "TEXT", nullable: false),
                    ProductDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    CycleTime = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductCode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    InitialDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    ProductCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductCode",
                        column: x => x.ProductCode,
                        principalTable: "Products",
                        principalColumn: "ProductCode");
                });

            migrationBuilder.CreateTable(
                name: "ProductMaterials",
                columns: table => new
                {
                    ProductCode = table.Column<string>(type: "TEXT", nullable: false),
                    MaterialCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaterials", x => new { x.ProductCode, x.MaterialCode });
                    table.ForeignKey(
                        name: "FK_ProductMaterials_Materials_MaterialCode",
                        column: x => x.MaterialCode,
                        principalTable: "Materials",
                        principalColumn: "MaterialCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMaterials_Products_ProductCode",
                        column: x => x.ProductCode,
                        principalTable: "Products",
                        principalColumn: "ProductCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    OrderId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaterialCode = table.Column<string>(type: "TEXT", nullable: true),
                    CycleTime = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productions", x => new { x.Id, x.Email, x.OrderId, x.Date });
                    table.ForeignKey(
                        name: "FK_Productions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productions_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "MaterialCode", "MaterialDescription" },
                values: new object[,]
                {
                    { "aaa", "desc1" },
                    { "bbb", "desc2" },
                    { "ccc", "desc3" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductCode", "CycleTime", "Image", "ProductDescription" },
                values: new object[,]
                {
                    { "abc", 30.3m, "0x000001", "xxx" },
                    { "def", 45.5m, "0x00000", "yyy" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "EndDate", "InitialDate", "Name" },
                values: new object[] { "teste@sqr.com.br", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test User" });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "ProductCode", "Quantity" },
                values: new object[,]
                {
                    { "111", "abc", 100.00m },
                    { "222", "def", 200.00m }
                });

            migrationBuilder.InsertData(
                table: "ProductMaterials",
                columns: new[] { "MaterialCode", "ProductCode" },
                values: new object[,]
                {
                    { "aaa", "abc" },
                    { "bbb", "abc" },
                    { "bbb", "def" },
                    { "ccc", "def" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductCode",
                table: "Orders",
                column: "ProductCode");

            migrationBuilder.CreateIndex(
                name: "IX_Productions_Email",
                table: "Productions",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Productions_OrderId",
                table: "Productions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaterials_MaterialCode",
                table: "ProductMaterials",
                column: "MaterialCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productions");

            migrationBuilder.DropTable(
                name: "ProductMaterials");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
