namespace ContosoUniversity.Features.Department
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.QueryResponse.Administrator>();
            CreateMap<Instructor, Create.QueryResponse.Instructor>();
            CreateMap<Department, Index.QueryResponse.Department>();
            CreateMap<Create.Command, Department>();
            CreateMap<Department, DepartmentDetailsQueryResponse>();
            CreateMap<Department, Edit.QueryResponse>();
        }
    }
}