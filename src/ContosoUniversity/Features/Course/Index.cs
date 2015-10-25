namespace ContosoUniversity.Features.Course
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using Models;

    public class Index
    {
        public class Query : IAsyncRequest<Query.Result>
        {
            public Department SelectedDepartment { get; set; }

            public class Result
            {
                public Department SelectedDepartment { get; set; }
                public List<Course> Courses { get; set; }

                public class Course
                {
                    public int Id { get; set; }
                    public string Number { get; set; }
                    public string Title { get; set; }
                    public int Credits { get; set; }
                    public string DepartmentName { get; set; }
                }
            }
        }

        public class Handler : MediatorHandler<Query, Query.Result>
        {
            public Handler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<Query.Result> Handle(Query message)
            {
                var departmentId = message.SelectedDepartment?.Id;

                var courses = await DbContext.Courses
                    .Where(c => !departmentId.HasValue || c.DepartmentId == departmentId)
                    .OrderBy(d => d.Id)
                    .ProjectTo<Query.Result.Course>().ToListAsync();

                return new Query.Result
                {
                    Courses = courses,
                    SelectedDepartment = message.SelectedDepartment
                };
            }
        }
    }
}