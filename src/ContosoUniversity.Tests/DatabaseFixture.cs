
namespace ContosoUniversity.Tests
{
    using System;
    using Microsoft.Data.Entity;
    using Microsoft.Framework.DependencyInjection;
    using ContosoUniversity.DataAccess;
    using ContosoUniversity.Models;
    using Respawn;
    using Xunit;

    public class DatabaseFixture : IDisposable
    {
        private bool _dataChanged = true;

        public DatabaseFixture()
        {
            var services = new ServiceCollection();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ContosoUniversityTestContext>(options => options.UseSqlServer("Server = localhost; Database = ContosoUniversity2016.Tests; Trusted_Connection = True;"));

            ServiceProvider = services.BuildServiceProvider();

            InitializeData();
        }

        public ContosoUniversityContext GetDbContext()
        {
            var context = ServiceProvider.GetService<ContosoUniversityTestContext>();
            context.ChangesSaved += () => _dataChanged = true;
            return context;
        }

        public IServiceProvider ServiceProvider { get; private set; }
       
        public void Dispose()
        {
            // to do:  implement
        }

        public void InitializeData()
        {
            if (_dataChanged)
            {
                DeleteData();
                SeedData();
                _dataChanged = false;
            }
        }

        private void DeleteData()
        {
            var checkpoint = new Checkpoint
            {
                TablesToIgnore = new [] { "sysdiagrams" },
                SchemasToExclude = new[] { "RoundhousE" }
            };

            // todo:  get this from settings
            var connectionString =
                "Server=localhost;Database=ContosoUniversity2016.Tests;Trusted_Connection=True;MultipleActiveResultSets=true";

            checkpoint.Reset(connectionString);
        }

        private void SeedData()
        {
            var context = ServiceProvider.GetService<ContosoUniversityTestContext>();

            // instructors
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Person on; insert dbo.Person(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) values (1, 'Martinez', 'Rick', '2012-05-21', null, 'Instructor'); set identity_insert dbo.Person off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Person on; insert dbo.Person(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) values (3, 'Johanssen', 'Beth', '2013-02-19', null, 'Instructor'); set identity_insert dbo.Person off;");

            // students
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Person on; insert dbo.Person(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) values (2, 'Watney', 'Mark', null, '2014-08-21', 'Student'); set identity_insert dbo.Person off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Person on; insert dbo.Person(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) values (4, 'Vogel', 'Alex', null, '2015-01-11', 'Student'); set identity_insert dbo.Person off;");

            // departments
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Department on; insert dbo.Department(Id, Name, Budget, StartDate, InstructorId) values (1, 'Engineering', 1234567.89, '2010-07-04', 1); set identity_insert dbo.Department off;");

            // courses
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Course on; insert dbo.Course(Id, Number, Title, Credits, DepartmentId) values (1, '60A', 'Computer Science I', 3, 1); set identity_insert dbo.Course off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Course on; insert dbo.Course(Id, Number, Title, Credits, DepartmentId) values (2, '60B', 'Computer Science II', 3, 1); set identity_insert dbo.Course off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Course on; insert dbo.Course(Id, Number, Title, Credits, DepartmentId) values (3, '100A', 'Advanced Algorithms I', 4, 1); set identity_insert dbo.Course off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Course on; insert dbo.Course(Id, Number, Title, Credits, DepartmentId) values (4, '100B', 'Advanced Algorithms II', 4, 1); set identity_insert dbo.Course off;");

            // office assignments
            context.Database.ExecuteSqlCommand("insert dbo.OfficeAssignment(InstructorId, Location) values (1, '200 Evans Hall')");
            context.Database.ExecuteSqlCommand("insert dbo.OfficeAssignment(InstructorId, Location) values (2, '100 Cory Hall')");

            // course instructors
            context.Database.ExecuteSqlCommand("insert dbo.CourseInstructor(CourseId, InstructorId) values (1, 1)");
            context.Database.ExecuteSqlCommand("insert dbo.CourseInstructor(CourseId, InstructorId) values (3, 1)");
            context.Database.ExecuteSqlCommand("insert dbo.CourseInstructor(CourseId, InstructorId) values (1, 3)");

            // enrollments
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Enrollment on; insert dbo.Enrollment(Id, CourseId, StudentId, Grade) values(1, 1, 2, 0); set identity_insert dbo.Enrollment off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Enrollment on; insert dbo.Enrollment(Id, CourseId, StudentId, Grade) values(2, 1, 4, 2); set identity_insert dbo.Enrollment off;");
        }
    }

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
