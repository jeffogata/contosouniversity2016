namespace ContosoUniversity.Infrastructure
{
    using System.Threading.Tasks;
    using DataAccess;
    using Models;

    /* 
        need to figure out how to register the generic command handler, then
        this class can be made concrete and the concrete subclasses can be removed
    */
    public abstract class DeleteCommandHandler<T> : MediatorHandler<DeleteCommand<T>, int>
        where T : Entity, new()
    {
        protected DeleteCommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        public override async Task<int> Handle(DeleteCommand<T> message)
        {
            var target = new T { Id = message.Id };

            DbContext.Set<T>().Remove(target);

            return await DbContext.SaveChangesAsync();
        }
    }
}
