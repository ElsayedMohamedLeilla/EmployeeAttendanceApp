using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core.JustificationsTypes;

namespace Dawem.Models.AutoMapper.Core
{
    public class JustificationsTypeMapProfile : Profile
    {
        public JustificationsTypeMapProfile()
        {
            CreateMap<CreateJustificationsTypeDTO, JustificationsType>();
            CreateMap<UpdateJustificationsTypeDTO, JustificationsType>();
        }
    }
}
