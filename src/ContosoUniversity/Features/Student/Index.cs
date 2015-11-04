namespace ContosoUniversity.Features.Student
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using Models;

    public class Index
    {
        public enum SortOn
        {
            None,
            LastName,
            FirstName,
            EnrollmentDate
        }

        public class Query : IAsyncRequest<QueryResponse>
        {
            public SortOn? SortOn { get; set; }
            public bool? SortAscending { get; set; }
            public string SearchString { get; set; }
            public int? Page { get; set; }
        }

        public class QueryResponse
        {
            public SortOn CurrentSort { get; set; }
            public bool SortAscending { get; set; }
            public string SearchString { get; set; }
            public int Page { get; set; }
            public int PageCount { get; set; }
            public List<Student> Results { get; set; } 
            public class Student
            {
                public int Id { get; set; }

                [Display(Name = "First Name")]
                public string FirstName { get; set; }

                [Display(Name = "Last Name")]
                public string LastName { get; set; }

                [Display(Name = "Enrollment Date")]
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
                    CurrentSort = message.SortOn ?? SortOn.LastName,
                    SortAscending = message.SortAscending ?? true,
                    SearchString = message.SearchString,
                    Page = message.Page ?? 1,
                };

                var query = DbContext.Students.AsQueryable();

                if (!string.IsNullOrWhiteSpace(message.SearchString))
                {
                    query = query.Where(s => s.LastName.Contains(message.SearchString) 
                                          || s.FirstName.Contains(message.SearchString));
                }

                // count will be used to calculate pagecount later on
                var count = await query.CountAsync();

                Expression<Func<Student, IComparable>> sortExpression = null;

                switch (message.SortOn)
                {
                    case SortOn.LastName:
                        sortExpression = s => s.LastName;
                        break;
                    case SortOn.FirstName:
                        sortExpression = s => s.FirstName;
                        break;
                    case SortOn.EnrollmentDate:
                        sortExpression = s => s.EnrollmentDate;
                        break;
                    default:
                        sortExpression = s => s.LastName;
                        break;
                }

                query = response.SortAscending ? query.OrderBy(sortExpression) : query.OrderByDescending(sortExpression);

                var pageSize = 3;
                var pageNumber = (message.Page ?? 1);
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