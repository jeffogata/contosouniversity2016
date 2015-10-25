namespace ContosoUniversity.Features.Department
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using Models;

    public class Delete
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class QueryResponse
        {
            public int Id { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }

            public string Name { get; set; }
            public decimal Budget { get; set; }

            [Display(Name = "Start Date")]
            public DateTime? StartDate { get; set; }
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

                var result = Mapper.Map<QueryResponse>(department);

                return result;
            }
        }

        public class Command : IAsyncRequest<int>
        {
            public int Id { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var department = new Department {Id = message.Id};

                DbContext.Departments.Remove(department);

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}