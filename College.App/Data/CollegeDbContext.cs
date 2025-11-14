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
        
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RolePrivilegeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());


        }

        public DbSet<Student> Students { get; set; }

       public DbSet<User> Users { get; set; }
       public DbSet<Department> Department { get; set; }

       public DbSet<Role> Roles { get; set; }
 
       public DbSet<RolePrivilege> RolePrivileges { get; set; }

       public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

        public DbSet<UserType> UserTypes { get; set; }



    }
}
