using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;

namespace Dawem.Models.AutoMapper.Dawem.Core
{
    public class PermissionsTypeMapProfile : Profile
    {
        public PermissionsTypeMapProfile()
        {
            CreateMap<CreatePermissionTypeDTO, PermissionType>();
            CreateMap<UpdatePermissionTypeDTO, PermissionType>();
        }
    }
}
