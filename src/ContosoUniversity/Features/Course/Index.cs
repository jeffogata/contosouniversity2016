using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using MediatR;

namespace ContosoUniversity.Features.Course
{
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Microsoft.Data.Entity;

    public class Index
    {
        public class Query : IAsyncRequest<Result>
        {
            public Department SelectedDepartment { get; set; }
        }

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

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly ContosoUniversityContext _db;

            public Handler(ContosoUniversityContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query message)
            {
                int? departmentId = message.SelectedDepartment?.Id;

                var courses = await _db.Courses
                    .Where(c => !departmentId.HasValue || c.DepartmentId == departmentId)
                    .OrderBy(d => d.Id)
                    .ProjectTo<Result.Course>().ToListAsync();

                return new Result
                {
                    Courses = courses,
                    SelectedDepartment = message.SelectedDepartment
                };
            }
        }
    }
}
