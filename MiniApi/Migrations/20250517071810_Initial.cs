using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ord");

            migrationBuilder.EnsureSchema(
                name: "prod");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "ord",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ordername = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "prod",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orderitems",
                schema: "ord",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    productid = table.Column<Guid>(type: "uuid", nullable: false),
                    orderid = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orderitems", x => x.id);
                    table.ForeignKey(
                        name: "fk_orderitems_orders_orderid",
                        column: x => x.orderid,
                        principalSchema: "ord",
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_orderitems_orderid",
                schema: "ord",
                table: "orderitems",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "ix_orderitems_productid",
                schema: "ord",
                table: "orderitems",
                column: "productid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderitems",
                schema: "ord");

            migrationBuilder.DropTable(
                name: "products",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "ord");
        }
    }
}
