namespace ContosoUniversity.Features.Home
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.Data.Entity;
    using Newtonsoft.Json;

    public class About
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
        }

        public class QueryResponse
        {
            [JsonProperty("statistics")]
            [Display(Name = "Statistics")]
            public List<Statistic> Statistics { get; set; } 
            public class Statistic
            {
                [JsonProperty("enrollmentDate")]
                [Display(Name = "Enrollment Date")]
                public DateTime EnrollmentDate { get; set; }
                [JsonProperty("students")]
                [Display(Name = "Students")]
                public int StudentCount { get; set; }
            }
        }

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                var results = await DbContext.Students.GroupBy(
                    s => s.EnrollmentDate,
                    s => s.Id,
                    (key, s) => new QueryResponse.Statistic
                    {
                        EnrollmentDate = key.Value,
                        StudentCount = s.Count()
                    })
                    .OrderByDescending(x => x.EnrollmentDate)
                    .ToListAsync();

                return new QueryResponse {Statistics = results};
            }
        }
    }
}