namespace ContosoUniversity.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using MediatR;

    public abstract class MediatorHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        protected MediatorHandler(ContosoUniversityContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            DbContext = dbContext;
        }

        protected ContosoUniversityContext DbContext { get; private set; }

        public abstract Task<TResponse> Handle(TRequest message);
    }
}