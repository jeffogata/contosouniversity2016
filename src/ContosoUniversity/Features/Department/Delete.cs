namespace ContosoUniversity.Features.Department
{
    using System.Threading.Tasks;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Models;

    public class Delete
    {
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