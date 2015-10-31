namespace ContosoUniversity.Features.Course
{
    using DataAccess;
    using Infrastructure;
    using Models;

    public class CourseDeleteCommandHandler : DeleteCommandHandler<Course>
    {
        public CourseDeleteCommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }
    }
}