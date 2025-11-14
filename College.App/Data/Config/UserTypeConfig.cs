using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            // Fluent API configurations for the UserType entity to make a proper database schema
            builder.ToTable("UserTypes"); // Table name
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn(); // Auto-incrementing primary key
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasData(new List<UserType>
            {
                new UserType { Id = 1, Name = "Student", Description = "For Student" },
                new UserType { Id = 2, Name = "Faculty", Description = "For Faculty" },
                new UserType { Id = 3, Name = "Supporting staff", Description = "For Supporting Staff" },
                new UserType { Id = 4, Name = "Parents", Description = "For Parent" }

            });

          
        }
    }
}
