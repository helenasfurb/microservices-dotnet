
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Model.Context
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext() { }
        public SqlServerContext(DbContextOptions<SqlServerContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Name = "Product 1",
                Price = new decimal(10.0),
                Description = "Description for Product 1",
                CategoryName = "Category 1",
                ImageUrl = "https://github.com/helenasfurb/microservices-dotnet/blob/main/ShoppingImages/00_no_image.jpg"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                Name = "Product 2",
                Price = new decimal(10.0),
                Description = "Description for Product 2",
                CategoryName = "Category 2",
                ImageUrl = "https://github.com/helenasfurb/microservices-dotnet/blob/main/ShoppingImages/10_milennium_falcon.jpg"
            });
        }
    }
}
