namespace ContosoUniversity.Features.Department
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.QueryResponse.Administrator>();
            CreateMap<Instructor, DepartmentCreateQueryResponse.Instructor>();
            CreateMap<Department, Index.QueryResponse.Department>();
            CreateMap<Create.Command, Department>();
            CreateMap<Department, Details.QueryModel>();
            CreateMap<Department, Edit.QueryResponse>();
        }
    }
}