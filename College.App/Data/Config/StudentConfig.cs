using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace College.App.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // Fluent API configurations for the Student entity to make a proper database schema
            builder.ToTable("Students"); // Table name
            builder.HasKey(e => e.Id);
            builder.Property(e => e.StudentName)
                .IsRequired()                 // NOT NULL
                .HasMaxLength(50)             // VARCHAR(50)
                .HasColumnType("varchar(50)");
            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType("varchar(100)");
            builder.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");
            builder.Property(e => e.DOB)
                .IsRequired()
                .HasColumnType("DateTime");

            // Seed data
            //You use OnModelCreating() + HasData() in Entity Framework Core to seed default data into your database automatically during migrations

            builder.HasData(
            new Student
            {
                Id = 1,
                StudentName = "Gayan Gunawardhana",
                Email = "gayan@gmail.com",
                Address = "Colombo",
                DOB = new DateTime(1998, 5, 21)
            },
            new Student
            {
                Id = 2,
                StudentName = "Kamal Perera",
                Email = "kamal@gmail.com",
                Address = "Kandy",
                DOB = new DateTime(1997, 11, 10)

            });
            // Configure the relationship between Student and Department
            builder.HasOne(e => e.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(e => e.DepartmentId)
                .HasConstraintName("FK_Student_Department");
               // .OnDelete(DeleteBehavior.SetNull); // When a Department is deleted, set DepartmentId to null in Student


        }
    }
}
