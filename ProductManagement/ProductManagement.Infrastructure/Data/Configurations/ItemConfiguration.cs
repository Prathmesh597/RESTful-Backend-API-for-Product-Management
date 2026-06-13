using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            // 1. Specifies the exact name of the database table to map the item entity to
            builder.ToTable("Item");

            // 2. Defines the primary key for the item table
            builder.HasKey(x => x.Id);

            // 3. Configures the quantity column to be a mandatory field in the database
            builder.Property(x => x.Quantity)
                .IsRequired();

            // 4. Configures the associated product ID column to be a mandatory field
            builder.Property(x => x.ProductId)
                .IsRequired();

        }
    }
}