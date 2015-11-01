namespace ContosoUniversity.Infrastructure
{
    using System.Threading.Tasks;
    using DataAccess;
    using Models;

    public abstract class CreateQueryHandler<TEntity, TResponse> :
        MediatorHandler<CreateQuery<TEntity, TResponse>, TResponse>
        where TEntity : Entity
        where TResponse : new()
    {
        protected CreateQueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        public override async Task<TResponse> Handle(CreateQuery<TEntity, TResponse> message)
        {
            var response = new TResponse();

            await ModifyResponse(response);

            return response;
        }

        protected virtual Task ModifyResponse(TResponse response)
        {
            return Task.CompletedTask;
        }
    }
}