using AutoMapper;

namespace Spendings.Data.Records
{
    public class RecordDaoProfile : Profile
    {
        public RecordDaoProfile()
        {
            CreateMap<Record, Core.Records.Contracts.Record>()
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, memberOptions: opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date))
                .ReverseMap();
        }
    }
}
