namespace ContosoUniversity.Features.Student
{
    using DataAccess;
    using Infrastructure;
    using Models;

    public class Delete
    {
        public class Command : DeleteCommand
        {
        }

        public class CommandHandler : DeleteCommandHandler<Student, Command>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }
        }
    }
}
