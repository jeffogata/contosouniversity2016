namespace ContosoUniversity.Features.Course
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
                var course = new Course {Id = message.Id};

                DbContext.Courses.Remove(course);

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}