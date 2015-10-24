namespace ContosoUniversity.Features.Department
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.Administrator>();
            CreateMap<Department, Index.Model>();
            CreateMap<Create.Command, Department>();
        }
    }
}