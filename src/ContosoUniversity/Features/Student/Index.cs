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

    public class Index
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public string SortOrder { get; set; }
            public string CurrentFilter { get; set; }
            public string SearchString { get; set; }
            public int? Page { get; set; }
        }

        public class QueryResponse
        {
            public string CurrentSort { get; set; }
            public string NameSortParm { get; set; }
            public string DateSortParm { get; set; }
            public string CurrentFilter { get; set; }
            public string SearchString { get; set; }
            public int Page { get; set; }
            public int PageCount { get; set; }
            public List<Student> Results { get; set; } 
            public class Student
            {
                public int Id { get; set; }

                [Display(Name = "First Name")]
                public string FirstName { get; set; }

                public string LastName { get; set; }
                public DateTime? EnrollmentDate { get; set; }
            }
        }

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var response = new QueryResponse
                {
                    CurrentSort = message.SortOrder,
                    NameSortParm = string.IsNullOrEmpty(message.SortOrder) ? "name_desc" : "",
                    DateSortParm = message.SortOrder == "Date" ? "date_desc" : "Date",
                    Page = message.Page ?? 1,
                    SearchString = message.SearchString
                };

                var query = DbContext.Students.AsQueryable();

                if (!string.IsNullOrWhiteSpace(message.SearchString))
                {
                    query = query.Where(s => s.LastName.Contains(message.SearchString) 
                                          || s.FirstName.Contains(message.SearchString));
                }

                switch (message.SortOrder)
                {
                    case "name_desc":
                        query = query.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        query = query.OrderBy(s => s.EnrollmentDate);
                        break;
                    case "date_desc":
                        query = query.OrderByDescending(s => s.EnrollmentDate);
                        break;
                    default: // Name ascending 
                        query = query.OrderBy(s => s.LastName);
                        break;
                }

                var pageSize = 3;
                var pageNumber = (message.Page ?? 1);
                var count = await query.CountAsync();
                var pageCount = (count + pageSize - 1) / pageSize;

                query = query.Skip((pageNumber - 1)*pageSize).Take(pageSize);

                var students = await query.ToListAsync();

                // problems using ProjectTo on Person/Student hierarchy
                response.Results = Mapper.Map<List<QueryResponse.Student>>(students);
                response.PageCount = pageCount;
                return response;
            }
        }
    }
}