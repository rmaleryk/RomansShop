﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RomansShop.Domain;

namespace RomansShop.DataAccess
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
                .HasKey(product => product.Id);

        }
    }
}