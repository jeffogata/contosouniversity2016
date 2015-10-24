using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Features.Department
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Models;

    public class Create
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
        }

        public class QueryResponse
        {
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal? Budget { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? StartDate { get; set; }

            [Display(Name = "Administrator")]
            public int? InstructorId { get; set; }

            //public List<Instructor> Instructors { get; set; }
            public List<SelectListItem> Instructors { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, QueryResponse>
        {
            private readonly ContosoUniversityContext _dbContext;

            public QueryHandler(ContosoUniversityContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<QueryResponse> Handle(Query message)
            {
                /*
                    wanted to do:

                        var instructors = await _dbContext.Instructors
                            .OrderBy(i => i.LastName)
                            .ProjectTo<Instructor>()
                            .ToListAsync();

                    but this returns instructors and students.  This
                    works fine in the HS Contoso example against EF 6.
                */

                var instructors = await _dbContext
                    .Instructors
                    .OrderBy(i => i.LastName)
                    .ToListAsync();

                var result = Mapper.Map<List<Instructor>>(instructors);

                List<SelectListItem> items = result.Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    }).ToList();
                return new QueryResponse
                {
                    Instructors = items
                };
            }
        }

        public class Command : IAsyncRequest
        {
            [StringLength(50, MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal? Budget { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? StartDate { get; set; }

            public int? InstructorId { get; set; }
        }

        public class Instructor
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName
            {
                get { return $"{LastName}, {FirstName}"; }
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ContosoUniversityContext _context;

            public CommandHandler(ContosoUniversityContext context)
            {
                _context = context;
            }

            protected async override Task HandleCore(Command message)
            {
                var department = Mapper.Map<Command, Department>(message);

                _context.Departments.Add(department);

                await _context.SaveChangesAsync();
            }
        }
    }
}
