using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RomansShop.Domain;

namespace RomansShop.DataAccess
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProduct();

            base.OnModelCreating(modelBuilder);
        }
    }
}
