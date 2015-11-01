using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Features.Instructor
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
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
            [Display(Name = "Last Name")]
            public string FirstName { get; set; }

            [Required, DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Hire Date")]
            public DateTime HireDate { get; set; }

            [Display(Name = "Office Location")]
            public string OfficeAssignmentLocation { get; set; }

            public List<Course> AvailableCourses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Number { get; set; }
                public string Title { get; set; }
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
                    AvailableCourses = courses
                };
            }
        }

        /*
        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("number")]
            public string Number { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("credits")]
            public int Credits { get; set; }

            [JsonProperty("departmentId")]
            public int DepartmentId { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var course = Mapper.Map<Command, Course>(message);

                DbContext.Courses.Add(course);

                return await DbContext.SaveChangesAsync();
            }
        }
        */
    }
}
