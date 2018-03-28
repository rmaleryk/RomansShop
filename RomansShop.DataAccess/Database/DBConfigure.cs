using Microsoft.EntityFrameworkCore;
using RomansShop.Domain.Entities;

namespace RomansShop.DataAccess.Database
{
    public static class DBConfigure
    {
        public static void ConfigureProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("products");

            modelBuilder.Entity<Product>()
                .Property(product => product.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Product>()
                .Property(product => product.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Product>()
                .Property(product => product.Description)
                .HasColumnName("description");

            modelBuilder.Entity<Product>()
                .Property(product => product.Price)
                .HasColumnName("price");

            modelBuilder.Entity<Product>()
                .Property(product => product.Quantity)
                .HasColumnName("quantity");

            modelBuilder.Entity<Product>()
                .Property(product => product.CategoryId)
                .HasColumnName("categoryid");

            modelBuilder.Entity<Product>()
                .HasKey(product => product.Id);
        }

        public static void ConfigureCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("categories");

            modelBuilder.Entity<Category>()
                .Property(category => category.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Category>()
                .Property(category => category.Name)
                .HasColumnName("name");

            modelBuilder.Entity<Category>()
                .HasKey(category => category.Id);
        }

        public static void ConfigureUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");

            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .HasColumnName("id");

            modelBuilder.Entity<User>()
                .Property(user => user.FullName)
                .HasColumnName("fullName");

            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasColumnName("email");

            modelBuilder.Entity<User>()
                .Property(user => user.Password)
                .HasColumnName("password");

            modelBuilder.Entity<User>()
                .Property(user => user.Rights)
                .HasColumnName("rights");

            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);
        }

        public static void ConfigureOrder(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("orders");

            modelBuilder.Entity<Order>()
                .Property(order => order.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Order>()
                .Property(order => order.UserId)
                .HasColumnName("userId");

            modelBuilder.Entity<Order>()
                .Property(order => order.CustomerEmail)
                .HasColumnName("customerEmail");

            modelBuilder.Entity<Order>()
                .Property(order => order.CustomerName)
                .HasColumnName("customerName");

            modelBuilder.Entity<Order>()
                .Property(order => order.Address)
                .HasColumnName("address");

            modelBuilder.Entity<Order>()
                .Property(order => order.Price)
                .HasColumnName("price");

            modelBuilder.Entity<Order>()
                .Property(order => order.Date)
                .HasColumnName("date");

            modelBuilder.Entity<Order>()
                .Property(order => order.Status)
                .HasColumnName("status");

            modelBuilder.Entity<Order>()
                .HasKey(order => order.Id);
        }

        public static void ConfigureOrderProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().ToTable("order_products");

            modelBuilder.Entity<OrderProduct>()
               .Property(op => op.Id)
               .HasColumnName("id");

            modelBuilder.Entity<OrderProduct>()
                .Property(op => op.OrderId)
                .HasColumnName("orderId");

            modelBuilder.Entity<OrderProduct>()
                .Property(op => op.ProductId)
                .HasColumnName("productId");

            modelBuilder.Entity<OrderProduct>().HasKey(op => op.Id);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(order => order.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(prod => prod.OrderProducts)
                .HasForeignKey(op => op.ProductId);
        }
    }
}