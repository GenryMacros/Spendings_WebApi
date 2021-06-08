using AutoMapper;

namespace Spendings.Orchrestrators.Records.Contracts
{
    public class RecordProfile : Profile
    {
        public RecordProfile()
        {
            CreateMap<Core.Records.Contracts.Record, InRecord>()
                .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date))
                .ReverseMap();

            CreateMap<Core.Records.Contracts.Record, OutRecord>()
                .ForMember(dest => dest.Id, memberOptions: opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.CategoryId, memberOptions: opt => opt.MapFrom(src => src.CategoryId))
               .ForMember(dest => dest.Amount, memberOptions: opt => opt.MapFrom(src => src.Amount))
               .ForMember(dest => dest.Date, memberOptions: opt => opt.MapFrom(src => src.Date));
        }
    }
}
