namespace ContosoUniversity.Features.Instructor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DataAccess;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNet.Mvc.Rendering;
    using Microsoft.Data.Entity;
    using Models;
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

        public class QueryResponse : Create.QueryResponse
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
                // getting an error here when using ProjectTo<QueryResponse>
                var instructor = await DbContext.Instructors
                    .Include(i => i.OfficeAssignment)
                    .SingleOrDefaultAsync(x => x.Id == message.Id);

                var response = Mapper.Map<QueryResponse>(instructor);

                // this returns the correct left-joined result set, but if i try to project to 
                // a Create.QueryResponse.Course instance, projecting to Assigned based on ci being non-null
                // throws a null reference exception (the line:  Assignment = ci != null)
                // for now, work around by projecting using L2O
                // todo: create a simple repro and log bug


                var courses = await (
                    from c in DbContext.Courses
                    from ci in DbContext.Set<CourseInstructor>()
                                        .Where(ci => ci.CourseId == c.Id && ci.InstructorId == message.Id)
                                        .DefaultIfEmpty()
                    orderby c.Number
                    select new 
                    {
                        Course = c,
                        CourseInstructor = ci
                    }).ToListAsync();

                response.AvaliableCourses = courses.Select(c =>
                    new Create.QueryResponse.Course
                    {
                        Id = c.Course.Id,
                        Number = c.Course.Number,
                        Title = c.Course.Title,
                        Assigned = c.CourseInstructor != null
                    }).ToList();

                return response;
            }
        }

        /*
        public class Command : IAsyncRequest<int>
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("budget")]
            public decimal Budget { get; set; }
            [JsonProperty("startDate")]
            public DateTime StartDate { get; set; }
            [JsonProperty("instructorId")]
            public int? InstructorId { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                var department = DbContext.Departments.FirstOrDefault(x => x.Id == message.Id);

                // todo: need to handle not found

                if (department != null)
                {
                    department.Name = message.Name;
                    department.Budget = message.Budget;
                    department.StartDate = message.StartDate;
                    department.InstructorId = message.InstructorId;

                    return await DbContext.SaveChangesAsync();
                }

                return 0;
            }
        }
        */
    }
}
