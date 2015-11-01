namespace ContosoUniversity.Features.Department
{
    using DataAccess;
    using Infrastructure;
    using Models;

    public class Delete
    {
        public class Command : DeleteCommand
        {
        }

        public class CommandHandler : DeleteCommandHandler<Department, Command>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }
        }
    }
}