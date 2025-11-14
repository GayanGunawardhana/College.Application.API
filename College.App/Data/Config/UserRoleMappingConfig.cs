using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            // Fluent API configurations for the UserRoleMapping entity to make a proper database schema
            builder.ToTable("UserRoleMappings"); // Table name
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn(); // Auto-incrementing primary key
            builder.HasIndex(e => new { e.UserId, e.RoleId },"UK_UserRoleMapping")
                .IsUnique();
            builder.Property(e => e.UserId)
                .IsRequired();
            builder.Property(e => e.RoleId)
                .IsRequired();

            builder.HasOne(urm => urm.User)
                .WithMany(u => u.UserRoleMappings)
                .HasForeignKey(urm => urm.UserId)
                .HasConstraintName("FK_UserRoleMapping_User");

            builder.HasOne(urm => urm.Role)
                .WithMany(r => r.UserRoleMappings)
                .HasForeignKey(urm => urm.RoleId)
                .HasConstraintName("FK_UserRoleMapping_Role");

        }

    }
}
