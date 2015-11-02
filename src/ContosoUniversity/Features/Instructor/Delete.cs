namespace ContosoUniversity.Features.Instructor
{
    using DataAccess;
    using Infrastructure;
    using Models;

    public class Delete
    {
        public class Command : DeleteCommand
        {
        }

        public class CommandHandler : DeleteCommandHandler<Instructor, Command>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }
        }
    }
}