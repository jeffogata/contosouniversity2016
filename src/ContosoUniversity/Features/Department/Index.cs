using Newtonsoft.Json;

namespace ContosoUniversity.Features.Department
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
        }

        public class QueryResponse : List<QueryResponse.Department>
        {
            public class Administrator
            {
                [Display(Name = "Administrator")]
                public string FullName { get; set; }
            }

            public class Department
            {
                [JsonProperty("id")]
                public int Id { get; set; }
                [JsonProperty("name")]
                public string Name { get; set; }
                [JsonProperty("budget")]
                public decimal Budget { get; set; }
                [JsonProperty("startDate")]
                public DateTime StartDate { get; set; }
                [JsonProperty("administratorFullName")]
                public string AdministratorFullName { get; set; }
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
                    wanted to use ProjectTo, but that generates an inner join, returning only the 
                    departments that have an Administrator:
              
                        return await _context
                            .Departments
                            .ProjectTo<Department>()
                            .ToListAsync();
                    
                    tried doing .Departments.Include(x => x.Administrator), but that didn't matter.
                    maybe the models/mapping can be modified to generate an outer join, but i don't
                    want to spend the time on that now
                */

                var departments = await DbContext
                    .Departments
                    .Include(x => x.Administrator)
                    .ToListAsync();

                return Mapper.Map<QueryResponse>(departments);
            }
        }
    }
}