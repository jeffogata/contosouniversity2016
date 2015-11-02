namespace ContosoUniversity.Features.Instructor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DataAccess;
    using Infrastructure;
    using MediatR;
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

        public class Command : Create.Command
        {
            [JsonProperty("id")]
            public int Id { get; set; }
        }

        public class CommandHandler : MediatorHandler<Command, int>
        {
            public CommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
            {
            }

            public override async Task<int> Handle(Command message)
            {
                /* Including CourseInstructors throws an exception:

                        SqlException: Invalid column name 'Id'.
                        System.Data.SqlClient.SqlCommand.<> c__DisplayClass16.< ExecuteDbDataReaderAsync > b__17(Task`1 result)

                        AggregateException: One or more errors occurred.
                        System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)

                    todo:  create a simple repro and capture the generated sql
                */

                var instructor = await DbContext.Instructors
                    .Include(i => i.OfficeAssignment)
                    //.Include(i => i.CourseInstructors)
                    .SingleOrDefaultAsync(i => i.Id == message.Id);

                // todo:  handle not found

                instructor.LastName = message.LastName;
                instructor.FirstName = message.FirstName;
                instructor.HireDate = message.HireDate;

                UpdateOfficeAssignment(message.OfficeAssignmentLocation, instructor);
                UpdateAssignedCourses(message.SelectedCourses, instructor);

                return await DbContext.SaveChangesAsync();
            }

            private void UpdateOfficeAssignment(string location, Instructor instructor)
            {
                if (location == null && instructor.OfficeAssignment == null)
                {
                    return;
                }

                if (instructor.OfficeAssignment == null)
                {
                    instructor.OfficeAssignment = new OfficeAssignment();
                }

                instructor.OfficeAssignment.Location = location;
            }

            private void UpdateAssignedCourses(List<int> assigned, Instructor instructor)
            {
                var courses = DbContext.CourseInstructors;

                if (assigned == null || !assigned.Any())
                {
                    courses.RemoveRange(courses.Where(x => x.InstructorId == instructor.Id));
                    return;
                }

                courses.RemoveRange(courses.Where(x => x.InstructorId == instructor.Id && !assigned.Contains(x.CourseId)));
                courses.AddRange(
                    assigned
                        .Except(
                            courses.Where(c => c.InstructorId == instructor.Id)
                                .Select(c => c.CourseId))
                        .Select(x => new CourseInstructor {InstructorId = instructor.Id, CourseId = x}));
            }
        }
    }
}