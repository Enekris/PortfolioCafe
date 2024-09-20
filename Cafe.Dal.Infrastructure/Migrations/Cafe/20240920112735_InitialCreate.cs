using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafe.Dal.Infrastructure.Migrations.Cafe
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DataCreate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__3214EC079C6F0AE3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    DataCreate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    Reserved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reserved_Customer_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tables__3214EC073EB625F0", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_id = table.Column<int>(type: "int", nullable: false),
                    Table_id = table.Column<int>(type: "int", nullable: false),
                    Order_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Close_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__3214EC07793E0129", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Orders__Customer__114A936A",
                        column: x => x.Customer_id,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Orders__Tables_i__123EB7A3",
                        column: x => x.Table_id,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdersDishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_id = table.Column<int>(type: "int", nullable: true),
                    Dish_id = table.Column<int>(type: "int", nullable: true),
                    Order_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DishPriceOnOrdersDate = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrdersDi__3214EC076E270CAB", x => x.Id);
                    table.ForeignKey(
                        name: "FK__OrdersDis__DishP__690797E6",
                        column: x => x.Order_id,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__OrdersDis__Dish___69FBBC1F",
                        column: x => x.Dish_id,
                        principalTable: "Dishes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__5C7E359E41EBE8F4",
                table: "Customers",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__A9D1053487236DEB",
                table: "Customers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ItemName",
                table: "Dishes",
                column: "ItemName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer_id",
                table: "Orders",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Table_id",
                table: "Orders",
                column: "Table_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDishes_Dish_id",
                table: "OrdersDishes",
                column: "Dish_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDishes_Order_id",
                table: "OrdersDishes",
                column: "Order_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Tables__78A1A19D5DB70977",
                table: "Tables",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersDishes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
