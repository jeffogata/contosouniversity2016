namespace ContosoUniversity.Features.Course
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Course, Index.QueryResponse.Course>();
            CreateMap<Course, Details.QueryResponse>();
        }
    }
}
