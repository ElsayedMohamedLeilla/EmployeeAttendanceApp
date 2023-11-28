using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core.Zones;

namespace Dawem.Models.AutoMapper
{
    public class ZoneMapProfile : Profile
    {
        public ZoneMapProfile()
        {
            CreateMap<CreateZoneDTO, Zone>();
            CreateMap<UpdateZoneDTO, Zone>();
        }
    }
}
