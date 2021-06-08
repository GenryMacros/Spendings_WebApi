using AutoMapper;
namespace Spendings.Data.Categories
{
    public class CategoryDaoProfile : Profile
    {
        public CategoryDaoProfile()
        {
            CreateMap<Category, Core.Categories.Contracts.Category>()
                .ForMember(dest => dest.Name, memberOptions: opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }

    }
}
