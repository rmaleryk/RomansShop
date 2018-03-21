using Microsoft.EntityFrameworkCore;
using RomansShop.Domain.Entities;

namespace RomansShop.DataAccess.Database
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProduct();
            modelBuilder.ConfigureCategory();
            modelBuilder.ConfigureUser();

            base.OnModelCreating(modelBuilder);
        }
    }
}