namespace ContosoUniversity.Features.Student
{
    using AutoMapper;
    using Models;

    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Student, Index.QueryResponse.Student>();
            CreateMap<Student, Details.QueryResponse>();
            CreateMap<Enrollment, Details.QueryResponse.StudentEnrollment>();
            CreateMap<Create.Command, Student>();
        }
    }
}