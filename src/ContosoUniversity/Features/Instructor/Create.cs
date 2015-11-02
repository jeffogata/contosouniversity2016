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
    using Newtonsoft.Json;

    public class Create
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
        }

        public class QueryResponse
        {
            [Required, StringLength(50)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required, StringLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required, DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Hire Date")]
            public DateTime? HireDate { get; set; }

            [Display(Name = "Office Location")]
            public string OfficeAssignmentLocation { get; set; }

            public List<Course> AvaliableCourses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Number { get; set; }
                public string Title { get; set; }
                public bool Assigned { get; set; }
            }
        }

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var courses = await DbContext.Courses
                    .OrderBy(c => c.Number)
                    .ProjectTo<QueryResponse.Course>()
                    .ToListAsync();

                return new QueryResponse
                {
                    HireDate = DateTime.Now.Date,
                    AvaliableCourses = courses
                };
            }
        }

        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("hireDate")]
            public DateTime HireDate { get; set; }

            [JsonProperty("officeLocation")]
            public string OfficeAssignmentLocation { get; set; }

            [JsonProperty("selectedCourses")]
            public List<int> SelectedCourses { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var instructor = Mapper.Map<Instructor>(message);

                if (message.OfficeAssignmentLocation != null)
                {
                    instructor.OfficeAssignment = new OfficeAssignment {Location = message.OfficeAssignmentLocation};
                }

                if (message.SelectedCourses?.Any() == true)
                {
                    instructor.CourseInstructors =
                        message.SelectedCourses.Select(c => new CourseInstructor {CourseId = c}).ToList();
                }

                DbContext.Instructors.Add(instructor);

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}