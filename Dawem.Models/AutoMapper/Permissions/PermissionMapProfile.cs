using AutoMapper;
using Dawem.Domain.Entities.Permissions;
using Dawem.Domain.Entities.Requests;
using Dawem.Enums.Generals;
using Dawem.Models.Dtos.Permissions.Permissions;
using Dawem.Models.Dtos.Requests.Assignments;

namespace Dawem.Models.AutoMapper.Permissions
{
    public class PermissionMapProfile : Profile
    {
        public PermissionMapProfile()
        {
            CreateMap<CreatePermissionModel, Permission>();
            CreateMap<UpdatePermissionModel, Permission>();
        }
    }
}
