namespace ContosoUniversity.Features.Course
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using Newtonsoft.Json;

    public class Details
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class QueryResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("number")]
            public string Number { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("credits")]
            public int Credits { get; set; }

            [JsonProperty("departmentName")]
            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
        }

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var course = await DbContext.Courses
                    .Include(x => x.Department)
                    .FirstOrDefaultAsync(x => x.Id == message.Id);

                var result = Mapper.Map<QueryResponse>(course);

                return result;
            }
        }
    }
}