using AutoMapper;
using Tickets.Domain.Entities;
using Tickets.ViewModel;

namespace Tickets.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Club, ClubVM>()
                .ForMember(dest => dest.Stadion,
                opts => opts
                .MapFrom(src => src.Stadion.StadionNaam))
                .ForMember(dest => dest.Capaciteit, opts => opts
                .MapFrom(src => src.Stadion.StadionCapaciteit));

            CreateMap<Wedstrijd, WedstrijdVM>()
                .ForMember(dest => dest.Thuisploeg,
                opts => opts
                .MapFrom(src => src.Thuisploeg.Clubnaam))
                .ForMember(dest => dest.Uitploeg, opts => opts
                .MapFrom(src => src.Uitploeg.Clubnaam));

            
        }
    }
}
