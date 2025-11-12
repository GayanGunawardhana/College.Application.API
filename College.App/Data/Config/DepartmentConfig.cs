using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // Fluent API configurations for the Department entity to make a proper database schema
            builder.ToTable("Departments"); // Table name
            builder.HasKey(e => e.DepartmentId);
            builder.Property(e => e.DepartmentName)
                .IsRequired()                 // NOT NULL
                .HasMaxLength(200)             // VARCHAR(50)
                .HasColumnType("varchar(200)");
           builder.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");
          

            // Seed data
            //You use OnModelCreating() + HasData() in Entity Framework Core to seed default data into your database automatically during migrations

            builder.HasData(
            new Department
            {
                DepartmentId = 1,
                DepartmentName = "SD",
                Description = "Software Department"
               
            },
            new Department
            {
                DepartmentId = 2,
                DepartmentName = "AD",
                Description = "Account Department"
                 

            });
        }
    }
}
