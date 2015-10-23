using AutoMapper;

namespace ContosoUniversity.Features.Department
{
    public class MappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Models.Department, Index.Model>();
        }
    }
}