using AutoMapper;

namespace Spendings.Orchrestrators.Categories.Contracts
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Core.Categories.Contracts.Category, InCategory>()
                .ForMember(dest => dest.Name, memberOptions: opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<Core.Categories.Contracts.Category, OutCategory>()
                .ForMember(dest => dest.Name, memberOptions: opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id));
        }
    }
}
