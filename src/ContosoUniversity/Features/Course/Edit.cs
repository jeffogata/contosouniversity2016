namespace ContosoUniversity.Features.Course
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Newtonsoft.Json;

    public class Edit
    {
        public class Query : IAsyncRequest<QueryResponse>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        public class QueryResponse : CourseCreateQueryResponse
        {
            public int Id { get; set; }
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

                var response = Mapper.Map<QueryResponse>(course);

                var departments = await DbContext
                    .Departments
                    .OrderBy(d => d.Name)
                    .ProjectTo<CourseCreateQueryResponse.Department>()
                    .ToListAsync();

                var items = departments.Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                var nullItem = new SelectListItem
                {
                    Value = "",
                    Text = ""
                };

                items.Insert(0, nullItem);

                response.Departments = items;

                return response;
            }
        }

        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("number")]
            public string Number { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("credits")]
            public int Credits { get; set; }

            [JsonProperty("departmentId")]
            public int DepartmentId { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var course = DbContext.Courses.FirstOrDefault(x => x.Id == message.Id);

                // todo: need to handle not found

                if (course != null)
                {
                    course.Number = message.Number;
                    course.Title = message.Title;
                    course.Credits = message.Credits;
                    course.DepartmentId = message.DepartmentId;

                    return await DbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
    }
}