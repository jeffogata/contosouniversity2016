using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Infrastructure
{
    using DataAccess;
    using MediatR;

    public abstract class MediatorRequestHandler<TRequest, TResponse>
        : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        protected MediatorRequestHandler(ContosoUniversityContext dbContext)
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
