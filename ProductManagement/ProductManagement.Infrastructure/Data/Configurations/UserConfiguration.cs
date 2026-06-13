using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // 1. Configures the database schema rules and constraints for the User entity
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // 2. Maps the entity to a specific database table named "Users"
            builder.ToTable("Users");

            // 3. Designates the Id property as the primary key for the table
            builder.HasKey(x => x.Id);

            // 4. Sets the Username field as required with a maximum limit of 50 characters
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(50);

            // 5. Enforces the Email property to be mandatory and capped at 100 characters
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            // 6. Marks the password hash field as required so it cannot be saved as null
            builder.Property(x => x.PasswordHash)
                .IsRequired();

            // 7. Restricts the user role designation to a maximum of 20 characters and makes it required
            builder.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(20);

            // 8. Allocates up to 500 characters for the optional refresh token storage
            builder.Property(x => x.RefreshToken)
                .HasMaxLength(500);

            // 9. Ensures the account creation timestamp must be provided when saving
            builder.Property(x => x.CreatedOn)
                .IsRequired();

            // 10. Applies a unique database index to the username to prevent duplicate registrations
            builder.HasIndex(x => x.Username)
                .IsUnique();

            // 11. Enforces uniqueness on the email address column across all user records
            builder.HasIndex(x => x.Email)
                .IsUnique();

        }

    }

}