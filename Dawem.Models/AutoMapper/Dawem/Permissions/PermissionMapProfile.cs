using AutoMapper;
using Dawem.Domain.Entities.Permissions;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;

namespace Dawem.Models.AutoMapper.Dawem.Permissions
{
    public class PermissionMapProfile : Profile
    {
        public PermissionMapProfile()
        {
            CreateMap<PermissionScreenModel, PermissionScreen>();
            CreateMap<PermissionScreenActionModel, PermissionScreenAction>();

            CreateMap<CreatePermissionModel, Permission>();
            CreateMap<UpdatePermissionModel, Permission>();
        }
    }
}
