using Microsoft.EntityFrameworkCore;
using RomansShop.Domain.Entities;

namespace RomansShop.DataAccess.Database
{
    public class ShopDbContext : DbContext
    {
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProduct();
            modelBuilder.ConfigureCategory();
            modelBuilder.ConfigureUser();
            modelBuilder.ConfigureOrder();
            modelBuilder.ConfigureOrderProduct();

            base.OnModelCreating(modelBuilder);
        }
    }
}