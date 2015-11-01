namespace ContosoUniversity.Features.Department
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Newtonsoft.Json;

    public class Edit
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class QueryResponse : Create.QueryResponse
        {
            public int Id { get; set; }
        }

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var department = await DbContext.Departments
                    .Include(x => x.Administrator)
                    .FirstOrDefaultAsync(x => x.Id == message.Id);

                var response = Mapper.Map<QueryResponse>(department);

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

                var mappedInstructors = Mapper.Map<List<Create.QueryResponse.Instructor>>(instructors);

                var items = mappedInstructors.Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    }).ToList();

                var nullItem = new SelectListItem
                {
                    Value = "",
                    Text = "",
                    Selected = response?.InstructorId.HasValue != true
                };

                items.Insert(0, nullItem);
                response.Instructors = items;

                return response;
            }
        }

        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("budget")]
            public decimal Budget { get; set; }
            [JsonProperty("startDate")]
            public DateTime StartDate { get; set; }
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
                var department = DbContext.Departments.FirstOrDefault(x => x.Id == message.Id);

                // todo: need to handle not found

                if (department != null)
                {
                    department.Name = message.Name;
                    department.Budget = message.Budget;
                    department.StartDate = message.StartDate;
                    department.InstructorId = message.InstructorId;

                    return await DbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
    }
}