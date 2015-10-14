
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

            context.Database.ExecuteSqlCommand("set identity_insert dbo.Person on; insert dbo.Person(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) values (1, 'Martinez', 'Rick', '2012-05-21', null, 'Instructor'); set identity_insert dbo.Person off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Person on; insert dbo.Person(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) values (2, 'Watney', 'Mark', null, '2014-08-21', 'Student'); set identity_insert dbo.Person off;");
            context.Database.ExecuteSqlCommand("set identity_insert dbo.Department on; insert dbo.Department(Id, Name, Budget, StartDate, InstructorId) values (1, 'Engineering', 1234567.89, '2010-07-04', 1); set identity_insert dbo.Department off;");
        }
    }

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
