namespace ContosoUniversity.Features.Department
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using MediatR;
    using Microsoft.Data.Entity;

    public class Index
    {
        public class Query : IAsyncRequest<List<Model>>
        {
        }

        public class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Budget { get; set; }
            public DateTime StartDate { get; set; }
            //public Administrator Administrator { get; set; }
            public string AdministratorFullName { get; set; }
        }

        public class Administrator
        {
            [Display(Name = "Administrator")]
            public string FullName { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, List<Model>>
        {
            private readonly ContosoUniversityContext _context;

            public QueryHandler(ContosoUniversityContext context)
            {
                _context = context;
            }

            public async Task<List<Model>> Handle(Query message)
            {
                /* 
                    wanted to use ProjectTo, but that generates an inner join, returning only the 
                    departments that have an Administrator:
              
                        return await _context
                            .Departments
                            .ProjectTo<Model>()
                            .ToListAsync();
                    
                    tried doing .Departments.Include(x => x.Administrator), but that didn't matter.
                    maybe the models/mapping can be modified to generate an outer join, but i don't
                    want to spend the time on that now
                */

                var departments = await _context
                    .Departments
                    .Include(x => x.Administrator)
                    .ToListAsync();

                return Mapper.Map<List<Model>>(departments);
            }
        }
    }
}