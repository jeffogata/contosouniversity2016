namespace ContosoUniversity.Features.Student
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using PagedList;

    public class Index
    {
        public class Query : IAsyncRequest<Query.Result>
        {
            public string SortOrder { get; set; }
            public string CurrentFilter { get; set; }
            public string SearchString { get; set; }
            public int? Page { get; set; }

            public class Result
            {
                public string CurrentSort { get; set; }
                public string NameSortParm { get; set; }
                public string DateSortParm { get; set; }
                public string CurrentFilter { get; set; }
                public string SearchString { get; set; }
                public IPagedList<Student> Results { get; set; }
            }

            public class Student
            {
                public int ID { get; set; }

                [Display(Name = "First Name")]
                public string FirstName { get; set; }

                public string LastName { get; set; }
                public DateTime? EnrollmentDate { get; set; }
            }
        }

        public class QueryHandler : MediatorHandler<Query, Query.Result>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<Query.Result> Handle(Query message)
            {
                var model = new Query.Result
                {
                    CurrentSort = message.SortOrder,
                    NameSortParm = string.IsNullOrEmpty(message.SortOrder) ? "name_desc" : "",
                    DateSortParm = message.SortOrder == "Date" ? "date_desc" : "Date"
                };

                if (message.SearchString != null)
                {
                    message.Page = 1;
                }
                else
                {
                    message.SearchString = message.CurrentFilter;
                }

                model.CurrentFilter = message.SearchString;
                model.SearchString = message.SearchString;

                var students = from s in DbContext.Students
                    select s;

                if (!string.IsNullOrEmpty(message.SearchString))
                {
                    students = students.Where(s => s.LastName.Contains(message.SearchString)
                                                   || s.FirstName.Contains(message.SearchString));
                }
                switch (message.SortOrder)
                {
                    case "name_desc":
                        students = students.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        students = students.OrderBy(s => s.EnrollmentDate);
                        break;
                    case "date_desc":
                        students = students.OrderByDescending(s => s.EnrollmentDate);
                        break;
                    default: // Name ascending 
                        students = students.OrderBy(s => s.LastName);
                        break;
                }

                var pageSize = 3;
                var pageNumber = (message.Page ?? 1);

                var source = await students.ToListAsync();

                // problems using ProjectTo on Person/Student hierarchy
                var mapped = Mapper.Map<List<Query.Student>>(source);

                model.Results = mapped.AsQueryable().ToPagedList(pageNumber, pageSize);

                return model;
            }
        }
    }
}