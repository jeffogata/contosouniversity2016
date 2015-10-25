namespace ContosoUniversity.Features.Instructor
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.QueryResponse.Instructor>();
            CreateMap<CourseInstructor, Index.QueryResponse.CourseInstructor>();
            CreateMap<Course, Index.QueryResponse.Course>();
            CreateMap<Enrollment, Index.QueryResponse.Enrollment>();
        }
    }
}
