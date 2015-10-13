namespace ContosoUniversity.Tests.IntegrationTests
{
    using System;
    using System.Linq;
    using Microsoft.Data.Entity;
    using Microsoft.Framework.DependencyInjection;

    using ContosoUniversity.DataAccess;
    using ContosoUniversity.Models;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Xunit;
    using Ploeh.AutoFixture.Xunit;

    [Collection("DatabaseCollection")]
    public class DepartmentTests
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly DatabaseFixture _fixture;

        public DepartmentTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeData();        
        }

        [Fact]
        public void TestDepartment()
        {
            using (var db = _fixture.GetDbContext())
            {
                var department = db.Departments.Single(x => x.Id == 1);
            }
        }
    }
}