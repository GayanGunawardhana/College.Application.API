using College.App.Data.Config;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace College.App.Data
{
    public class CollegeDbContext : DbContext
    {
        public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options)
        {


        }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This code is not generated automatically.
            //Table 01: Students
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());

        }
        
       public DbSet<Student> Students { get; set; }
       public DbSet<Department> Department { get; set; }


    }
}
