namespace ContosoUniversity.Features.Course
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Models;
    using Newtonsoft.Json;

    public class CourseCreateQueryResponse
    {
        [StringLength(5)]
        [Required]
        public string Number { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Required]
        public int Credits { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }

        public List<SelectListItem> Departments { get; set; }

        public class Department
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }

    public class CourseCreateQueryHandler : CreateQueryHandler<Course, CourseCreateQueryResponse>
    {
        public CourseCreateQueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        protected override async Task ModifyResponse(CourseCreateQueryResponse response)
        {
            var departments = await DbContext
                .Departments
                .OrderBy(d => d.Name)
                .ProjectTo<CourseCreateQueryResponse.Department>()
                .ToListAsync();

            var items = departments.Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

            var nullItem = new SelectListItem
            {
                Value = "",
                Text = "",
                Selected = true
            };

            items.Insert(0, nullItem);

            response.Departments = items;
        }
    }

    public class Create
    {
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
    }
}