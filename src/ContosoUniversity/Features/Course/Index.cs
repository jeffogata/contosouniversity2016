namespace ContosoUniversity.Features.Course
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;

    public class Index
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public Query()
            {
            }

            public Query(int departmentId)
            {
                DepartmentId = departmentId;
            }

            public int? DepartmentId { get; set; }
        }

        public class QueryResponse
        {
            public int? DepartmentId { get; set; }

            public List<SelectListItem> Departments { get; set; } 

            public List<Course> Courses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Number { get; set; }
                public string Title { get; set; }
                public int Credits { get; set; }
                [Display(Name = "Department")]
                public string DepartmentName { get; set; }
            }

            public class Department
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }

        public class Handler : MediatorHandler<Query, QueryResponse>
        {
            public Handler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var departmentId = message.DepartmentId;

                var courses = await DbContext.Courses
                    .Where(c => !departmentId.HasValue || c.DepartmentId == departmentId)
                    .OrderBy(d => d.Id)
                    .ProjectTo<QueryResponse.Course>().ToListAsync();

                // project to SelectListItems until we find/create a better way
                // (I don't like having UI concerns in the handler)
                var departments = (await DbContext.Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new { Id = d.Id, Name = d.Name })
                    .ToListAsync())
                    // doing a second projection on the results due to issue in EF7 handling nullable values 
                    // see https://github.com/aspnet/EntityFramework/issues/2450 
                    // and https://github.com/aspnet/EntityFramework/issues/3618
                    // - EF7 is generating invalid SQL
                    .Select(x => new SelectListItem
                        {
                            Selected = x.Id == departmentId,
                            Value = x.Id.ToString(),
                            Text = x.Name
                        })
                    .ToList();

                departments.Insert(0, new SelectListItem { Value = "", Text = "All" });

                return new QueryResponse
                {
                    Departments = departments,
                    DepartmentId = departmentId,
                    Courses = courses
                };
            }
        }
    }
}