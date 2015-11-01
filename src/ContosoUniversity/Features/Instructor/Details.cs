namespace ContosoUniversity.Features.Instructor
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataAccess;
    using Infrastructure;
    using Microsoft.Data.Entity;
    using Models;
    using Newtonsoft.Json;

    public class Details
    {
        public class Query : DetailsQuery<QueryResponse>
        {
            public Query(int id) : base(id)
            {
            }
        }

        public class QueryResponse
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("location")]
            [Display(Name = "Location")]
            public string OfficeAssignmentLocation { get; set; }

            [JsonProperty("lastName")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [JsonProperty("firstName")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [JsonProperty("hireDate")]
            [Display(Name = "Hire Date")]
            public DateTime HireDate { get; set; }
        }

        public class QueryHandler : DetailsQueryHandler<Instructor, Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            protected override IQueryable<Instructor> ModifyQuery(IQueryable<Instructor> query)
            {
                return query.Include(x => x.OfficeAssignment);
            }
        }
    }
}