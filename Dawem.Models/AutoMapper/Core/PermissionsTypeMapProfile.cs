using AutoMapper;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Core.PermissionsTypes;

namespace Dawem.Models.AutoMapper.Core
{
    public class PermissionsTypeMapProfile : Profile
    {
        public PermissionsTypeMapProfile()
        {
            CreateMap<CreatePermissionsTypeDTO, PermissionsType>();
            CreateMap<UpdatePermissionsTypeDTO, PermissionsType>();
        }
    }
}
