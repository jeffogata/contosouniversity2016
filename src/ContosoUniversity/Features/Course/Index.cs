using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;
using MediatR;

namespace ContosoUniversity.Features.Course
{
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
                public string Title { get; set; }
                public int Credits { get; set; }
                public string DepartmentName { get; set; }
            }
        }
    }
}
