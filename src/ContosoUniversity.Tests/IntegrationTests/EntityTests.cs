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
    public class EntityTests
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly DatabaseFixture _fixture;

        public EntityTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeData();        
        }

        [Fact]
        public void SelectDepartmentById_ReturnsDepartment()
        {
            Department department = null;

            using (var db = _fixture.GetDbContext())
            {
                department = db.Departments.Include(x => x.Administrator).Single(x => x.Id == 1);
            }

            Assert.Equal("Engineering", department.Name);
            Assert.Equal(1234567.89m, department.Budget);
            Assert.Equal(new DateTime(2010, 7, 4), department.StartDate);
            Assert.Equal(1, department.InstructorId);
            Assert.Equal(1, department.Administrator.Id);
        }

        [Fact]
        public void SelectInstructorById_ReturnsInstructor()
        {
            Instructor instructor = null;

            using (var db = _fixture.GetDbContext())
            {
                instructor = db.Instructors.Single(x => x.Id == 1);
            }

            Assert.Equal("Martinez", instructor.LastName);
            Assert.Equal("Rick", instructor.FirstName);
            Assert.Equal(new DateTime(2012, 5, 21), instructor.HireDate);
        }
    }
}