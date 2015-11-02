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
            // todo: revisit use of FK actions when deleting instructors
            // i updated the FKS to set dept admin to null and cascade delete of office assignment. the only practical 
            // reason i see to do this outside of the database is to support a RDBMS that doesn't support cascading 
            // FK actions, but even SQLite supports SET NULL and CASCADE: https://www.sqlite.org/foreignkeys.html#fk_actions

            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }
        }
    }
}