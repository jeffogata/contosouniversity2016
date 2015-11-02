namespace ContosoUniversity.Features.Instructor
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Instructor, Index.QueryResponse.Instructor>();
            CreateMap<Course, Index.QueryResponse.Course>();
            CreateMap<Enrollment, Index.QueryResponse.Enrollment>();
            CreateMap<Instructor, Details.QueryResponse>();
            CreateMap<Course, Create.QueryResponse.Course>();
            CreateMap<Create.Command, Instructor>();
            CreateMap<Instructor, Edit.QueryResponse>();
        }
    }
}
