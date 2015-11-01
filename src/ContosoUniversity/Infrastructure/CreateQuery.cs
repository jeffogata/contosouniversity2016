namespace ContosoUniversity.Infrastructure
{
    using MediatR;
    using Models;

    public class CreateQuery<TEntity, TResponse> : IAsyncRequest<TResponse>
        where TEntity : Entity
    {
    }
}
