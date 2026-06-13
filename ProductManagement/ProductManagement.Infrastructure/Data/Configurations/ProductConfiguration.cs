using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // 1. Specifies the exact name of the database table to map this entity to
            builder.ToTable("Product");

            // 2. Defines the primary key for the product table
            builder.HasKey(x => x.Id);

            // 3. Configures the product name column to be mandatory with a maximum character limit
            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(255);

            // 4. Configures the creator identifier column to be mandatory and limits its length
            builder.Property(x => x.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            // 5. Ensures the creation timestamp column is always required
            builder.Property(x => x.CreatedOn)
                .IsRequired();

            // 6. Limits the maximum length of the optional modifier identifier column
            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(100);

            // 7. Explicitly marks the modification timestamp column as optional
            builder.Property(x => x.ModifiedOn)
                .IsRequired(false);

            // 8. Establishes a one-to-many relationship with items, ensuring cascading deletion when a product is removed
            builder.HasMany(x => x.Items)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}