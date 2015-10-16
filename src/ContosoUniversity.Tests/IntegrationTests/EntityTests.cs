namespace ContosoUniversity.Tests.IntegrationTests
{
    using System;
    using System.Linq;
    using Ploeh.AutoFixture;
    using ContosoUniversity.Models;
    using Microsoft.Data.Entity;
    using Xunit;

    [Collection("DatabaseCollection")]
    public class EntityTests
    {
        private readonly DatabaseFixture _dbFixture;
        private readonly Fixture _autoFixture;

        public EntityTests(DatabaseFixture dbFixture)
        {
            _dbFixture = dbFixture;
            _dbFixture.InitializeData();

            _autoFixture = new Fixture();
            _autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void SelectDepartmentById_ReturnsDepartment()
        {
            Department department = null;

            using (var db = _dbFixture.GetDbContext())
            {
                department = db.Departments
                    .Include(x => x.Administrator)
                    .Include(x => x.Courses)
                    .Single(x => x.Id == 1);
            }

            Assert.Equal("Engineering", department.Name);
            Assert.Equal(1234567.89m, department.Budget);
            Assert.Equal(new DateTime(2010, 7, 4), department.StartDate);
            Assert.Equal(1, department.InstructorId);
            Assert.Equal(1, department.Administrator.Id);

            Assert.Equal(4, department.Courses.Count);
            Assert.True(department.Courses.Any(x => x.Number == "60A"));
            Assert.True(department.Courses.Any(x => x.Number == "60B"));
            Assert.True(department.Courses.Any(x => x.Number == "100A"));
            Assert.True(department.Courses.Any(x => x.Number == "100B"));
        }

        [Fact]
        public void AddUpdateDeleteDepartment_SavesChanges()
        {
            var department = _autoFixture.Build<Department>()
                .Without(d => d.Id)
                .Without(d => d.Administrator)
                .With(d => d.InstructorId, 1)
                .Without(d => d.Courses)
                .Create();
            
            using (var db = _dbFixture.GetDbContext())
            {
                db.Departments.Add(department);
                db.SaveChanges();

                var id = department.Id;

                var savedDepartment = db.Departments.Single(x => x.Id == id);

                Assert.Equal(department.Name, savedDepartment.Name);
                Assert.Equal(department.Budget, savedDepartment.Budget);
                Assert.Equal(department.StartDate, savedDepartment.StartDate);
                Assert.Equal(department.InstructorId, savedDepartment.InstructorId);

                var newName = Guid.NewGuid().ToString();
                var newBudget = savedDepartment.Budget - 1;
                var newStartDate = savedDepartment.StartDate.AddDays(-1);

                savedDepartment.Name = newName;
                savedDepartment.Budget = newBudget;
                savedDepartment.StartDate = newStartDate;
                savedDepartment.InstructorId = null;

                db.SaveChanges();

                var updatedDepartment = db.Departments.Single(x => x.Id == id);

                Assert.Equal(newName, updatedDepartment.Name);
                Assert.Equal(newBudget, updatedDepartment.Budget);
                Assert.Equal(newStartDate, updatedDepartment.StartDate);
                Assert.Null(updatedDepartment.InstructorId);

                db.Departments.Remove(updatedDepartment);

                db.SaveChanges();

                Assert.Null(db.Departments.SingleOrDefault(x => x.Id == id));
            }
        }

        [Fact]
        public void SelectInstructorById_ReturnsInstructor()
        {
            Instructor instructor = null;

            using (var db = _dbFixture.GetDbContext())
            {
                instructor = db.Instructors
                    .Include(x => x.OfficeAssignment)
                    .Single(x => x.Id == 1);
            }

            Assert.Equal("Martinez", instructor.LastName);
            Assert.Equal("Rick", instructor.FirstName);
            Assert.Equal(new DateTime(2012, 5, 21), instructor.HireDate);
            Assert.Equal("200 Evans Hall", instructor.OfficeAssignment.Location);
        }

        [Fact]
        public void SelectStudentById_ReturnsStudent()
        {
            Student student = null;

            using (var db = _dbFixture.GetDbContext())
            {
                student = db.Students
                    .Include(x => x.Enrollments)
                    .Single(x => x.Id == 2);
            }

            Assert.Equal("Watney", student.LastName);
            Assert.Equal("Mark", student.FirstName);
            Assert.Equal(new DateTime(2014, 8, 21), student.EnrollmentDate);
            Assert.Equal(1, student.Enrollments.Count);
            Assert.Equal(Grade.A, student.Enrollments.First().Grade);
        }

        [Fact]
        public void SelectCourseById_ReturnsCourse()
        {
            Course course = null;

            using (var db = _dbFixture.GetDbContext())
            {
                course = db.Courses
                    .Include(x => x.Department)
                    //.Include(x => x.CourseInstructors.Select(ci => ci.Instructor))
                    .Include(x => x.CourseInstructors).ThenInclude(ci => ci.Instructor) // new syntax!
                    .Include(x => x.Enrollments).ThenInclude(e => e.Student)
                    .Single(x => x.Id == 1);
            }

            Assert.Equal("60A", course.Number);
            Assert.Equal("Computer Science I", course.Title);
            Assert.Equal(3, course.Credits);
            Assert.Equal(1, course.DepartmentId);
            Assert.Equal("Engineering", course.Department.Name);

            Assert.Equal(2, course.CourseInstructors.Count);
            Assert.True(course.CourseInstructors.Any(x => x.Instructor.Id == 1));
            Assert.True(course.CourseInstructors.Any(x => x.Instructor.Id == 3));

            Assert.Equal(2, course.Enrollments.Count);

            var enrollment = course.Enrollments.Single(e => e.Student.Id == 2);

            Assert.Equal(Grade.A, enrollment.Grade);

            enrollment = course.Enrollments.Single(e => e.Student.Id == 4);

            Assert.Equal(Grade.C, enrollment.Grade);
        }

        [Fact]
        public void SelectOfficeAssignmentById_ReturnsOfficeAssignment()
        {
            OfficeAssignment office = null;

            using (var db = _dbFixture.GetDbContext())
            {
                office = db.OfficeAssignments
                    .Include(x => x.Instructor)
                    .Single(x => x.InstructorId == 1);
            }

            Assert.Equal("Martinez", office.Instructor.LastName);
            Assert.Equal("Rick", office.Instructor.FirstName);
            Assert.Equal("200 Evans Hall", office.Location);
        }
    }
}