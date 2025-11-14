using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
    {
        public void Configure(EntityTypeBuilder<RolePrivilege> builder)
        {
            // Fluent API configurations for the RolePrivilege entity to make a proper database schema
            builder.ToTable("RolePrivileges"); // Table name
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn(); // Auto-incrementing primary key
            builder.Property(e => e.RolePrivilegeName)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnType("nvarchar(250)");
            builder.Property(e => e.Description);
            builder.Property(e => e.RoleId)
                .IsRequired();
            builder.Property(e => e.IsActive)
                .IsRequired();
            builder.Property(e => e.CreatedDate)
                .IsRequired();
            builder.Property(e => e.ModifiedDate)
                .IsRequired();
            // Configure the relationship between RolePrivilege and Role
            builder.HasOne(n=> n.Role)
                .WithMany(n=> n.RolePrivileges)
                .HasForeignKey(n => n.RoleId)
                .HasConstraintName("FK_RolePrivilege_Role");
        }
    }
}
