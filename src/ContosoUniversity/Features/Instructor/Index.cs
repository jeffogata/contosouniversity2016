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
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using Models;

    public class Index
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public int? Id { get; set; }
            public int? CourseId { get; set; }
        }

        public class QueryResponse
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

        public class Handler : MediatorHandler<Query, QueryResponse>
        {
            public Handler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var instructors =
                    Mapper.Map<List<QueryResponse.Instructor>>(await DbContext.Instructors
                        .OrderBy(i => i.LastName)
                        .ToListAsync());

                var courses = new List<QueryResponse.Course>();
                var enrollments = new List<QueryResponse.Enrollment>();

                if (message.Id != null)
                {
                    courses = await DbContext.Courses
                        .Where(c => c.CourseInstructors.Any(ci => ci.InstructorId == message.Id))
                        .ProjectTo<QueryResponse.Course>()
                        .ToListAsync();
                }

                if (message.CourseId != null)
                {
                    enrollments = await DbContext.Enrollments
                        .Where(x => x.CourseId == message.CourseId)
                        .ProjectTo<QueryResponse.Enrollment>()
                        .ToListAsync();
                }

                var viewModel = new QueryResponse
                {
                    Instructors = instructors,
                    Courses = courses,
                    Enrollments = enrollments,
                    InstructorId = message.Id,
                    CourseId = message.CourseId
                };

                return viewModel;
            }
        }
    }
}