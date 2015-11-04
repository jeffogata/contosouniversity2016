namespace ContosoUniversity.Features.Student
{
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
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
                // issues with ProjectTo and TPH inheritance
                var student = await DbContext.Students
                    .SingleOrDefaultAsync(x => x.Id == message.Id);

                var response = Mapper.Map<QueryResponse>(student);

                return response;
            }
        }

        public class Command : Create.Command
        {
            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var student = await DbContext.Students
                    .SingleOrDefaultAsync(i => i.Id == message.Id);

                student.LastName = message.LastName;
                student.FirstName = message.FirstName;
                student.EnrollmentDate = message.EnrollmentDate;

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}