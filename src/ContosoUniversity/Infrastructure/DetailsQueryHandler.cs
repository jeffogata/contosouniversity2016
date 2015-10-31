namespace ContosoUniversity.Infrastructure
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Microsoft.Data.Entity;
    using Models;

    public abstract class DetailsQueryHandler<TEntity, TResponse> :
        MediatorHandler<DetailsQuery<TEntity, TResponse>, TResponse>
        where TEntity : Entity
    {
        protected DetailsQueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        public override async Task<TResponse> Handle(DetailsQuery<TEntity, TResponse> message)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();

            query = ModifyQuery(query);

            var entity = await query.FirstOrDefaultAsync(x => x.Id == message.Id);

            var result = Mapper.Map<TResponse>(entity);

            return result;
        }

        protected virtual IQueryable<TEntity> ModifyQuery(IQueryable<TEntity> query)
        {
            return query;
        }
    }
}