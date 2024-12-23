using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class OvertimeTypeMapProfile : Profile
    {
        public OvertimeTypeMapProfile()
        {
            CreateMap<CreateOvertimeTypeDTO, OvertimeType>();
            CreateMap<UpdateOvertimeTypeDTO, OvertimeType>();
        }
    }
}
