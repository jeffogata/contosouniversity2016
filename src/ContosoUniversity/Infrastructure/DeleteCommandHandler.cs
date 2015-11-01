namespace ContosoUniversity.Infrastructure
{
    using System.Threading.Tasks;
    using DataAccess;
    using Models;

    public abstract class DeleteCommandHandler<TEntity, TCommand> : MediatorHandler<TCommand, int>
        where TEntity : Entity, new()
        where TCommand : DeleteCommand
    {
        protected DeleteCommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        public override async Task<int> Handle(TCommand message)
        {
            var target = new TEntity { Id = message.Id };

            DbContext.Set<TEntity>().Remove(target);

            return await DbContext.SaveChangesAsync();
        }
    }
}
