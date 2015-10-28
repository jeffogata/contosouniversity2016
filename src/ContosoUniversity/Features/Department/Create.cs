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
    using Newtonsoft.Json;

    public class Create
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
        }

        public class QueryResponse
        {
            public string Name { get; set; }
            public decimal? Budget { get; set; }
            // need the DataType attribute to have the datepicker rendered
            [DataType(DataType.Date)]
            [Display(Name = "Start Date")]
            public DateTime? StartDate { get; set; }

            [Display(Name = "Administrator")]
            public int? InstructorId { get; set; }
            public List<SelectListItem> Instructors { get; set; }

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

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
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

                var result = Mapper.Map<List<QueryResponse.Instructor>>(instructors);

                var items = result.Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    }).ToList();


                var nullItem = new SelectListItem
                {
                    Value = "",
                    Text = "",
                    Selected = true
                };

                items.Insert(0, nullItem);

                return new QueryResponse
                {
                    StartDate = DateTime.Now.Date,
                    Instructors = items
                };
            }
        }

        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("budget")]
            public decimal? Budget { get; set; }
            [JsonProperty("startDate")]
            public DateTime? StartDate { get; set; }
            [JsonProperty("instructorId")]
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