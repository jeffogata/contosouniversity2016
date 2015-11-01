namespace ContosoUniversity.Infrastructure
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Microsoft.Data.Entity;
    using Models;

    public abstract class DetailsQueryHandler<TEntity, TQuery, TQueryModel> : MediatorHandler<TQuery, TQueryModel>
        where TEntity : Entity
        where TQuery : DetailsQuery<TQueryModel>
    {
        protected DetailsQueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }

        public override async Task<TQueryModel> Handle(TQuery message)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();

            query = ModifyQuery(query);

            var entity = await query.FirstOrDefaultAsync(x => x.Id == message.Id);

            var result = Mapper.Map<TQueryModel>(entity);

            return result;
        }

        protected virtual IQueryable<TEntity> ModifyQuery(IQueryable<TEntity> query)
        {
            return query;
        }
    }
}