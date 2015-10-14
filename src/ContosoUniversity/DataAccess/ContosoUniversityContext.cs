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
            modelBuilder.Entity<Student>()
                .BaseType<Person>();

            modelBuilder.Entity<Instructor>()
                .BaseType<Person>();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Course> Courses { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; } 

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        public DbSet<Student> Students { get; set; }
    }
}
