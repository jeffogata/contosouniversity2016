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

                [Display(Name = "Office")]
                public string OfficeAssignmentLocation { get; set; }

                public List<InstructorCourse>  Courses { get; set; }
            }

            public class InstructorCourse
            {
                public string CourseNumber { get; set; }
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

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                /* would like to include courses in this query, but the following query generates invalid SQL:

                    var temp = await DbContext.Instructors
                            .Include(i => i.OfficeAssignment)
                            .Include(i => i.CourseInstructors)
                            .OrderBy(i => i.LastName)
                            .ToListAsync();
                    
                    generates

                        SELECT [i].[Id], [i].[Discriminator], [i].[FirstName], [i].[LastName], [i].[HireDate], [o].[InstructorId], [o].[Location]
                        FROM [Person] AS [i]
                        LEFT JOIN [OfficeAssignment] AS [o] ON [o].[InstructorId] = [i].[Id]
                        WHERE [i].[Discriminator] = 'Instructor'
                        ORDER BY [i].[LastName], [o].[Id]

                    note: the error is on the ORDER BY column name of [o].[Id].  The correct name (InstructorId) is used in other places
                */

                var instructors =
                    Mapper.Map<List<QueryResponse.Instructor>>(await DbContext.Instructors
                        .Include(i => i.OfficeAssignment)
                        .OrderBy(i => i.LastName)
                        .ToListAsync());

                // *** workaround for not being able to select course info in the instructors query

                var instructorCourses = await DbContext.Set<CourseInstructor>().AsNoTracking()
                    .Select(ci => new { ci.InstructorId, ci.Course.Number, ci.Course.Title})
                    .ToListAsync();

                foreach (var instructor in instructors)
                {
                    instructor.Courses = instructorCourses.Where(ci => ci.InstructorId == instructor.Id)
                        .Select(ci => new QueryResponse.InstructorCourse {CourseNumber = ci.Number, CourseTitle = ci.Title})
                        .ToList();
                }

                // *** end workaround

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