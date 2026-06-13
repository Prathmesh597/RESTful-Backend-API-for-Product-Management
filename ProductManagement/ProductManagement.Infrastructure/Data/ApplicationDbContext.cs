using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Data.Configurations;
using System;

namespace ProductManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        // 1. Initializes the database context with the required connection and framework options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 2. Defines the database table used to access and manage product records
        public DbSet<Product> Products { get; set; }

        // 3. Defines the database table used to access and manage item records
        public DbSet<Item> Items { get; set; }

        // Defines the database table used to access and manage User records
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 4. Ensures the default model creation behavior from the base framework is executed
            base.OnModelCreating(modelBuilder);

            // 5. Applies custom database schema configurations and constraints for the defined entities
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.ApplyConfiguration(new ItemConfiguration());

            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}