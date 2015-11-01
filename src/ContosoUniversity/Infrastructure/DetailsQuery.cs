namespace ContosoUniversity.Infrastructure
{
    using MediatR;

    public abstract class DetailsQuery<TQueryModel> : IAsyncRequest<TQueryModel>
    {
        private readonly int _id;

        protected DetailsQuery(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }
    }
}
