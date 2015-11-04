namespace ContosoUniversity.Features.Student
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Models;
    using Newtonsoft.Json;

    public class Create
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
        }

        public class QueryResponse
        {
            [Required, StringLength(50)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required, StringLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required, DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Enrollment Date")]
            public DateTime? EnrollmentDate { get; set; }
        }

        public class QueryHandler : MediatorHandler<Query, QueryResponse>
        {
            public QueryHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<QueryResponse> Handle(Query message)
            {
                return new QueryResponse
                {
                    EnrollmentDate = DateTime.Now.Date
                };
            }
        }

        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("enrollmentDate")]
            public DateTime EnrollmentDate { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var student = Mapper.Map<Student>(message);

                DbContext.Students.Add(student);

                return await DbContext.SaveChangesAsync();
            }
        }
    }
}