namespace ContosoUniversity.Features.Course
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Course, Index.QueryResponse.Course>();
            CreateMap<Course, CourseDetailsQueryResponse>();
            CreateMap<Course, Edit.QueryResponse>();
            CreateMap<Department, CourseCreateQueryResponse.Department>();
            CreateMap<Create.Command, Course>();

        }
    }
}
