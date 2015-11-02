namespace ContosoUniversity.DataAccess
{
    using Microsoft.Data.Entity;
    using Microsoft.Data.Entity.Infrastructure;
    using Microsoft.Data.Entity.Metadata;

    using Models;

    public class ContosoUniversityContext : DbContext
    {
        public ContosoUniversityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // ignore RowVersion for now - we need to ignore or set to computed for insert/update to work
            // use this later when we add optimistic concurrency
            modelBuilder.Entity<Department>()
                .Ignore(x => x.RowVersion);

            modelBuilder.Entity<Student>()
                .BaseType<Person>();

            modelBuilder.Entity<Instructor>()
                .BaseType<Person>();

            // per this comment, first release of EF7 requires an entity for many-to-many relationships:
            // https://github.com/aspnet/EntityFramework/issues/1368#issuecomment-126129034
            // see http://stackoverflow.com/a/29474030/409259

            modelBuilder.Entity<CourseInstructor>()
                .HasKey(x => new {x.CourseId, x.InstructorId});

            modelBuilder.Entity<OfficeAssignment>()
                .HasOne(x => x.Instructor)
                .WithOne(x => x.OfficeAssignment)
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; } 

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<CourseInstructor> CourseInstructors { get; set; } 
    }
}
