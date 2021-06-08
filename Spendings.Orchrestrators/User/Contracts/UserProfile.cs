using AutoMapper;

namespace Spendings.Orchrestrators.Users.Contracts
{
    public class OrchUserProfile : Profile
    {
        public OrchUserProfile()
        {
            CreateMap<InUser,Core.Users.Contracts.User>()
                .ForMember(dest => dest.Login, memberOptions: opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, memberOptions: opt => opt.MapFrom(src => src.Password))
                .ReverseMap();

            CreateMap<Core.Users.Contracts.User, OutUser>()
               .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Login, memberOptions: opt => opt.MapFrom(src => src.Login))
               .ForMember(dest => dest.Password, memberOptions: opt => opt.MapFrom(src => src.Password));
        }
    }
  
}
