using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DevelopmentPathWays.Models
{
    public class DB_Entities: DbContext
    {
        public DB_Entities() : base("Pathways") { }
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DB_Entities>(null);
            modelBuilder.Entity<UsersModel>().ToTable("Users");
            modelBuilder.Entity<EmployeeModel>().ToTable("Employees");
            modelBuilder.Entity<DepartmentModel>().ToTable("Departments");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);

        }
    }
}