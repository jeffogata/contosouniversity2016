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

                var saved = db.Departments.Single(x => x.Id == id);

                Assert.Equal(department.Name, saved.Name);
                Assert.Equal(department.Budget, saved.Budget);
                Assert.Equal(department.StartDate, saved.StartDate);
                Assert.Equal(department.InstructorId, saved.InstructorId);

                var newName = Guid.NewGuid().ToString();
                var newBudget = saved.Budget - 1;
                var newStartDate = saved.StartDate.AddDays(-1);

                saved.Name = newName;
                saved.Budget = newBudget;
                saved.StartDate = newStartDate;
                saved.InstructorId = null;

                db.SaveChanges();

                var updated = db.Departments.Single(x => x.Id == id);

                Assert.Equal(newName, updated.Name);
                Assert.Equal(newBudget, updated.Budget);
                Assert.Equal(newStartDate, updated.StartDate);
                Assert.Null(updated.InstructorId);

                db.Departments.Remove(updated);

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
        public void AddUpdateDeleteInstructor_SavesChanges()
        {
            var instructor = _autoFixture.Build<Instructor>()
                .Without(i => i.Id)
                .Without(i => i.CourseInstructors)
                .Without(i => i.OfficeAssignment)
                .Create();

            using (var db = _dbFixture.GetDbContext())
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();

                var id = instructor.Id;

                var saved = db.Instructors.Single(x => x.Id == id);

                Assert.Equal(instructor.FirstName, saved.FirstName);
                Assert.Equal(instructor.LastName, saved.LastName);
                Assert.Equal(instructor.HireDate, saved.HireDate);

                var newFirstName = Guid.NewGuid().ToString();
                var newLastName = Guid.NewGuid().ToString();
                var newHireDate = (DateTime?)saved.HireDate.Value.AddDays(-1);

                saved.FirstName = newFirstName;
                saved.LastName = newLastName;
                saved.HireDate = newHireDate;

                db.SaveChanges();

                var updated = db.Instructors.Single(x => x.Id == id);

                Assert.Equal(newFirstName, updated.FirstName);
                Assert.Equal(newLastName, updated.LastName);
                Assert.Equal(newHireDate, updated.HireDate);

                db.Instructors.Remove(updated);

                db.SaveChanges();

                Assert.Null(db.Instructors.SingleOrDefault(x => x.Id == id));
            }
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
        public void AddUpdateDeleteStudent_SavesChanges()
        {
            var student = _autoFixture.Build<Student>()
                .Without(x => x.Id)
                .Without(x => x.Enrollments)
                .Create();

            using (var db = _dbFixture.GetDbContext())
            {
                db.Students.Add(student);
                db.SaveChanges();

                var id = student.Id;

                var saved = db.Students.Single(x => x.Id == id);

                Assert.Equal(student.FirstName, saved.FirstName);
                Assert.Equal(student.LastName, saved.LastName);
                Assert.Equal(student.EnrollmentDate, saved.EnrollmentDate);                

                var newFirstName = Guid.NewGuid().ToString();
                var newLastName = Guid.NewGuid().ToString();
                var newHireDate = (DateTime?)saved.EnrollmentDate.Value.AddDays(-1);

                saved.FirstName = newFirstName;
                saved.LastName = newLastName;
                saved.EnrollmentDate = newHireDate;

                db.SaveChanges();

                var updated = db.Students.Single(x => x.Id == id);

                Assert.Equal(newFirstName, updated.FirstName);
                Assert.Equal(newLastName, updated.LastName);
                Assert.Equal(newHireDate, updated.EnrollmentDate);

                db.Students.Remove(updated);

                db.SaveChanges();

                Assert.Null(db.Students.SingleOrDefault(x => x.Id == id));
            }
        }

        [Fact]
        public void SelectCourseById_ReturnsCourse()
        {
            Course course = null;

            using (var db = _dbFixture.GetDbContext())
            {
                course = db.Courses
                    .Include(x => x.Department)
                    .Include(x => x.CourseInstructors).ThenInclude(ci => ci.Instructor) // new syntax! old syntax -> //.Include(x => x.CourseInstructors.Select(ci => ci.Instructor))
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
        public void AddUpdateDeleteCourse_SavesChanges()
        {
            var course = _autoFixture.Build<Course>()
                .Without(x => x.Id)
                .Without(x => x.Department)
                .Without(x => x.CourseInstructors)
                .Without(x => x.Enrollments)
                .With(x => x.DepartmentId, 1)
                .With(x => x.Number, "abcde")
                .Create();

            using (var db = _dbFixture.GetDbContext())
            {
                db.Courses.Add(course);
                db.SaveChanges();

                var id = course.Id;

                var saved = db.Courses.Single(x => x.Id == id);

                Assert.Equal(course.Number, saved.Number);
                Assert.Equal(course.Title, saved.Title);
                Assert.Equal(course.Credits, saved.Credits);
                Assert.Equal(course.DepartmentId, saved.DepartmentId);

                var newNumber = "vwxyz";
                var newTitle = Guid.NewGuid().ToString();
                var newCredits = saved.Credits + 1;
                var newDepartmentId = 2;

                saved.Number = newNumber;
                saved.Title = newTitle;
                saved.Credits = newCredits;
                saved.DepartmentId = newDepartmentId;

                db.SaveChanges();

                var updated = db.Courses.Single(x => x.Id == id);

                Assert.Equal(newNumber, updated.Number);
                Assert.Equal(newTitle, updated.Title);
                Assert.Equal(newCredits, updated.Credits);
                Assert.Equal(newDepartmentId, updated.DepartmentId);

                db.Courses.Remove(updated);

                db.SaveChanges();

                Assert.Null(db.Courses.SingleOrDefault(x => x.Id == id));
            }
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

        [Fact]
        public void AddUpdateDeleteOfficeAssigment_SavesChanges()
        {
            var instructorId = 3;

            var office = _autoFixture.Build<OfficeAssignment>()
                .With(x => x.InstructorId, instructorId)
                .Without(x => x.Instructor)
                .Create();

            using (var db = _dbFixture.GetDbContext())
            {
                db.OfficeAssignments.Add(office);
                db.SaveChanges();

                var saved = db.OfficeAssignments.Single(x => x.InstructorId == instructorId);

                Assert.Equal(office.Location, saved.Location);

                var newLocation = Guid.NewGuid().ToString();

                saved.Location = newLocation;

                db.SaveChanges();

                var updated = db.OfficeAssignments.Single(x => x.InstructorId == instructorId);

                Assert.Equal(newLocation, updated.Location);

                db.OfficeAssignments.Remove(updated);

                db.SaveChanges();

                Assert.Null(db.OfficeAssignments.SingleOrDefault(x => x.InstructorId == instructorId));
            }
        }
    }
}