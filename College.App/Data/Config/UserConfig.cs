using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Fluent API configurations for the Student entity to make a proper database schema
            builder.ToTable("Users"); // Table name
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn(); // Auto-incrementing primary key
            builder.Property(e => e.Username)
                .IsRequired();                 
            builder.Property(e => e.Password)
                .IsRequired();
            builder.Property(e => e.PasswordSalt)
                .IsRequired();
            builder.Property(e => e.IsActive)
                .IsRequired();
            builder.Property(e => e.IsDelete)
                .IsRequired();
            builder.Property(e => e.CreatedDate)
                .IsRequired();
            builder.Property(e => e.ModifiedDate)
                .IsRequired();
            builder.Property(e => e.UserTypeId)
                .IsRequired();
            
            builder.HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .HasConstraintName("FK_User_UserType");

        }
    }
}
