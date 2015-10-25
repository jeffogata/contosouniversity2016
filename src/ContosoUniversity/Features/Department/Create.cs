namespace ContosoUniversity.Features.Department
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
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Models;

    public class Create
    {
        public class Query : IAsyncRequest<Query.Response>
        {
            public class Response
            {
                public string Name { get; set; }
                public decimal? Budget { get; set; }
                // need the DataType attribute to have the datepicker rendered
                [DataType(DataType.Date)]
                public DateTime? StartDate { get; set; }

                public int? InstructorId { get; set; }
                public List<SelectListItem> Instructors { get; set; }
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
        }

        public class QueryHandler : MediatorHandler<Query, Query.Response>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<Query.Response> Handle(Query message)
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

                var instructors = await DbContext
                    .Instructors
                    .OrderBy(i => i.LastName)
                    .ToListAsync();

                var result = Mapper.Map<List<Query.Instructor>>(instructors);

                var items = result.Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    }).ToList();

                return new Query.Response
                {
                    Instructors = items
                };
            }
        }

        public class Command : IAsyncRequest<int>
        {
            public string Name { get; set; }
            public decimal? Budget { get; set; }
            public DateTime? StartDate { get; set; }
            public int? InstructorId { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var department = Mapper.Map<Command, Department>(message);

                DbContext.Departments.Add(department);

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}