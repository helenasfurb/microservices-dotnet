using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeekShopping.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "id", "category_name", "description", "image_url", "name", "price" },
                values: new object[,]
                {
                    { 2L, "Category 1", "Description for Product 1", "https://github.com/helenasfurb/microservices-dotnet/blob/main/ShoppingImages/00_no_image.jpg", "Product 1", 10m },
                    { 3L, "Category 2", "Description for Product 2", "https://github.com/helenasfurb/microservices-dotnet/blob/main/ShoppingImages/10_milennium_falcon.jpg", "Product 2", 10m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "product",
                keyColumn: "id",
                keyValue: 3L);
        }
    }
}
