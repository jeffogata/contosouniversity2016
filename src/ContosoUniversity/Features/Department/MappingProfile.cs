namespace ContosoUniversity.Features.Department
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.Query.Administrator>();
            CreateMap<Instructor, Create.Query.Instructor>();
            CreateMap<Department, Index.Query.Department>();
            CreateMap<Create.Command, Department>();
            CreateMap<Department, Delete.Query.Response>();
        }
    }
}