namespace ContosoUniversity.Infrastructure
{
    using MediatR;
    using Models;

    // TEntity is not needed, but I believe it has value since it makes it explicit 
    // which entity set is being acted on when the command is sent

    public class DeleteCommand<TEntity> : IAsyncRequest<int>
        where TEntity : Entity
    {
        public int Id { get; set; }
    }
}
