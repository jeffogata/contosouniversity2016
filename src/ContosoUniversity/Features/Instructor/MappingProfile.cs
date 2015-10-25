namespace ContosoUniversity.Features.Instructor
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.Query.Response.Instructor>();
            CreateMap<CourseInstructor, Index.Query.Response.CourseInstructor>();
            CreateMap<Course, Index.Query.Response.Course>();
            CreateMap<Enrollment, Index.Query.Response.Enrollment>();
        }
    }
}
