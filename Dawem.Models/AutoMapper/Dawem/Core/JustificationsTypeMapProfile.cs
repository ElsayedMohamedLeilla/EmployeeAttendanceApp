using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.JustificationsTypes;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class JustificationsTypeMapProfile : Profile
    {
        public JustificationsTypeMapProfile()
        {
            CreateMap<CreateJustificationsTypeDTO, JustificationType>();
            CreateMap<UpdateJustificationsTypeDTO, JustificationType>();
        }
    }
}
