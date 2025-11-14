using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Fluent API configurations for the Role entity to make a proper database schema
            builder.ToTable("Roles"); // Table name
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn(); // Auto-incrementing primary key
            builder.Property(e => e.RoleName)
                .IsRequired();
            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(250)");
            builder.Property(e => e.IsActive)
                .IsRequired();
            builder.Property(e => e.IsDelete)
                .IsRequired();
            builder.Property(e => e.CreatedDate)
                .IsRequired();
            builder.Property(e => e.ModifiedDate)
                .IsRequired();
         

        }
    
    }
}
