namespace ContosoUniversity.Features.Department
{
    using DataAccess;
    using Infrastructure;
    using Models;

    public class DepartmentDeleteCommandHandler : DeleteCommandHandler<Department>
    {
        public DepartmentDeleteCommandHandler(ContosoUniversityContext dbContext) : base(dbContext)
        {
        }
    }
}