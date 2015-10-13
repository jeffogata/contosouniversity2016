using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.DataAccess
{
    using Microsoft.Data.Entity;
    using Models;

    public class ContosoUniversityContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Person> People { get; set; } 

        public DbSet<Instructor> Instructors { get; set; } 
    }
}
