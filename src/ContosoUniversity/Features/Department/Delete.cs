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

        public class Command : IAsyncRequest
        {
            public int Id { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ContosoUniversityContext _dbContext;

            public CommandHandler(ContosoUniversityContext dbContext)
            {
                _dbContext = dbContext;
            }

            protected override async Task HandleCore(Command message)
            {
                var department = new Department {Id = message.Id};

                _dbContext.Departments.Remove(department);

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}