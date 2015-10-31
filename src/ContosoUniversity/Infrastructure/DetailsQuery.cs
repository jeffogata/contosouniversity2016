namespace ContosoUniversity.Infrastructure
{
    using MediatR;
    using Models;

    public class DetailsQuery<TEntity, TResponse> : IAsyncRequest<TResponse>
        where TEntity : Entity
    {
        public DetailsQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
