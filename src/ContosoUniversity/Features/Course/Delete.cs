namespace ContosoUniversity.Features.Course
{
    using DataAccess;
    using Infrastructure;
    using Models;

    public class Delete
    {
        public class Command : DeleteCommand
        {
        }

        public class CommandHandler : DeleteCommandHandler<Course, Command>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }
        }
    }
}