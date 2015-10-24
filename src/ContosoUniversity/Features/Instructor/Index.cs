namespace ContosoUniversity.Features.Instructor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Mapping;
    using MediatR;
    using Microsoft.Data.Entity;
    using Models;

    public class Index
    {
        public class Query : IAsyncRequest<Model>
        {
            public int? Id { get; set; }
            public int? CourseID { get; set; }
        }

        public class Model
        {
            public int? InstructorId { get; set; }
            public int? CourseId { get; set; }
            public List<Instructor> Instructors { get; set; }
            public List<Course> Courses { get; set; }
            public List<Enrollment> Enrollments { get; set; }

            public class Instructor
            {
                public int Id { get; set; }

                [Display(Name = "Last Name")]
                public string LastName { get; set; }

                [Display(Name = "First Name")]
                public string FirstName { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
                [Display(Name = "Hire Date")]
                public DateTime? HireDate { get; set; }

                //public string OfficeAssignmentLocation { get; set; }
                //public IEnumerable<CourseInstructor> CourseInstructors { get; set; }
            }

            public class CourseInstructor
            {
                public int CourseId { get; set; }
                public string CourseTitle { get; set; }
            }

            public class Course
            {
                public int CourseId { get; set; }
                public string Title { get; set; }
                public string DepartmentName { get; set; }
            }

            public class Enrollment
            {
                [DisplayFormat(NullDisplayText = "No grade")]
                public Grade? Grade { get; set; }

                public string StudentFullName { get; set; }
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly ContosoUniversityContext _db;

            public Handler(ContosoUniversityContext db)
            {
                _db = db;
            }

            public async Task<Model> Handle(Query message)
            {
                var instructors = 
                    Mapper.Map<List<Model.Instructor>>(await _db.Instructors
                    .OrderBy(i => i.LastName)
                    .ToListAsync());

                var courses = new List<Model.Course>();
                var enrollments = new List<Model.Enrollment>();

                if (message.Id != null)
                {
                    courses = await _db.Courses
                        .Where(c => c.CourseInstructors.Any(ci => ci.InstructorId == message.Id))
                        .ProjectTo<Model.Course>()
                        .ToListAsync();
                }

                if (message.CourseID != null)
                {
                    enrollments = await _db.Enrollments
                        .Where(x => x.CourseId == message.CourseID)
                        .ProjectTo<Model.Enrollment>()
                        .ToListAsync();
                }

                var viewModel = new Model
                {
                    Instructors = instructors,
                    Courses = courses,
                    Enrollments = enrollments,
                    InstructorId = message.Id,
                    CourseId = message.CourseID
                };

                return viewModel;
            }
        }
    }
}