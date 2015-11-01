namespace ContosoUniversity.Features.Course
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataAccess;
    using Infrastructure;
    using Microsoft.Data.Entity;
    using Models;
    using Newtonsoft.Json;

    public class Details
    {
        public class QueryModel
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

        public class QueryHandler : DetailsQueryHandler<Course, QueryModel>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            protected override IQueryable<Course> ModifyQuery(IQueryable<Course> query)
            {
                return query.Include(x => x.Department);
            }
        }
    }
}