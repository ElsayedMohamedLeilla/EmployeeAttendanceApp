using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.Zones;

namespace Dawem.Models.AutoMapper.Dawem.Core
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
