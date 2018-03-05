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
    }
}