using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.DataAccess
{
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Metadata;

    using Models;

    public class ContosoUniversityContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .BaseType<Person>();

            modelBuilder.Entity<Instructor>()
                .BaseType<Person>();

            // per this comment, first release of EF7 requires an entity for many-to-many relationships:
            // https://github.com/aspnet/EntityFramework/issues/1368#issuecomment-126129034
            // see http://stackoverflow.com/a/29474030/409259

            modelBuilder.Entity<CourseInstructor>().Key(x => new {x.CourseId, x.InstructorId});

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
