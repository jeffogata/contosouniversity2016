namespace ContosoUniversity.Infrastructure
{
    using MediatR;

    public abstract class DeleteCommand : IAsyncRequest<int>
    {
        public int Id { get; set; }
    }
}
