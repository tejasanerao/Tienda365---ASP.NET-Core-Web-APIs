using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tienda365.DL.Entities;

namespace Tienda365.DL
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Oppo Reno 6",
                    Image = "oppo1.png",
                    MRPAmount = 15999,
                    DiscountPercentage = 0,
                    InStock = true,
                    MaxOrderAmount = 3,
                    InventoryId = "Mob-oppo-1",
                    CategoryId = 4
                },
                new Product
                {
                    Id = 2,
                    Name = "Vivo X80 Pro",
                    Image = "https://rukminim1.flixcart.com/image/832/832/l3929ow0/mobile/v/n/s/-original-imageewzeguggzvc.jpeg?q=70",
                    MRPAmount = 79999,
                    DiscountPercentage = 0,
                    InStock = true,
                    MaxOrderAmount = 3,
                    InventoryId = "Mob-vivo-1",
                    CategoryId = 4
                },
                new Product
                {
                    Id = 3,
                    Name = "Samsung M32",
                    Image = "https://m.media-amazon.com/images/I/71F4jU7MRUS._SL1500_.jpg",
                    MRPAmount = 15999,
                    DiscountPercentage = 0,
                    InStock = true,
                    MaxOrderAmount = 3,
                    InventoryId = "Mob-Sam-1",
                    CategoryId = 4
                },
                new Product
                {
                    Id = 4,
                    Name = "Iphone 13 Max pro",
                    Image = "https://www.apple.com/newsroom/images/product/iphone/standard/Apple_iPhone-13-Pro_iPhone-13-Pro-Max_09142021_inline.jpg.large.jpg",
                    MRPAmount = 15999,
                    DiscountPercentage = 0,
                    InStock = true,
                    MaxOrderAmount = 3,
                    InventoryId = "Mob-iphone-1",
                    CategoryId = 4
                });

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Books",
                    Description = "Test",

                },
                new Category
                {
                    Id = 2,
                    Name = "Games",
                    Description = "Test"
                },
                new Category
                {
                    Id = 3,
                    Name = "Tools",
                    Description = "Test"
                },
                new Category
                {
                    Id = 4,
                    Name = "Mobiles",
                    Description = "Test"
                },
                new Category
                {
                    Id = 5,
                    Name = "Laptops",
                    Description = "Test"
                }




            );

        }
    }
}
