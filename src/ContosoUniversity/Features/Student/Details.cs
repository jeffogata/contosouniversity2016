namespace ContosoUniversity.Features.Student
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using DataAccess;
    using Infrastructure;
    using Models;
    using Microsoft.Data.Entity;
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

            [JsonProperty("lastName")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [JsonProperty("firstName")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [JsonProperty("enrollmentDate")]
            [Display(Name = "Enrollment Date")]
            public DateTime EnrollmentDate { get; set; }

            [JsonProperty("enrollments")]
            public List<StudentEnrollment> Enrollments { get; set; }

            public class StudentEnrollment
            {
                [JsonProperty("courseTitle")]
                [Display(Name = "Course Title")]
                public string CourseTitle { get; set; }

                [JsonProperty("grade")]
                public Grade Grade { get; set; }
            }
        }

        public class QueryHandler : DetailsQueryHandler<Student, Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            protected override IQueryable<Student> ModifyQuery(IQueryable<Student> query)
            {
                return query.Include(x => x.Enrollments).ThenInclude(e => e.Course);
            }
        }
    }
}
