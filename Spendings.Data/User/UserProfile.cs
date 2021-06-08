
using AutoMapper;

namespace Spendings.Data.Users
{
    public class UserDaoProfile : Profile
    {
        public UserDaoProfile()
        {
            CreateMap<Core.Users.Contracts.User, User>()
                .ForMember(dest => dest.Login, memberOptions: opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, memberOptions: opt => opt.MapFrom(src => src.Password));

            CreateMap<User, Core.Users.Contracts.User>()
                .ForMember(dest => dest.Login, memberOptions: opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.Password, memberOptions: opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id));
        }
    }
}
