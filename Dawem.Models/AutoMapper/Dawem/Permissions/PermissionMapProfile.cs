using AutoMapper;
using Dawem.Domain.Entities.Permissions;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;

namespace Dawem.Models.AutoMapper.Dawem.Permissions
{
    public class PermissionMapProfile : Profile
    {
        public PermissionMapProfile()
        {
            CreateMap<PermissionScreenModel, PermissionScreen>();
            //CreateMap<PermissionScreenActionModel, PermissionScreenAction>();

            CreateMap<CreatePermissionModel, Permission>().
                ForMember(dest => dest.PermissionScreens, opts => opts.MapFrom(src => src.Screens)).
                AfterMap(MapPermissionScreens);
            CreateMap<UpdatePermissionModel, Permission>().
                ForMember(dest => dest.PermissionScreens, opts => opts.MapFrom(src => src.Screens)).
                AfterMap(MapPermissionScreens);
        }
        private void MapPermissionScreens(CreatePermissionModel source, Permission destination, ResolutionContext context)
        {
            if (destination.PermissionScreens != null)
            {
                foreach (var permissionScreen in destination.PermissionScreens)
                {
                    var getScreen = source.Screens.FirstOrDefault(s => s.ScreenId == permissionScreen.ScreenId);

                    if (getScreen != null && getScreen.Actions != null)
                    {
                        permissionScreen.PermissionScreenActions = new List<PermissionScreenAction>();
                        permissionScreen.PermissionScreenActions.AddRange(getScreen.Actions.Select(a=> new PermissionScreenAction
                        {
                            ActionCode = a
                        }).ToList());
                    }
                }
            }
        }
        private void MapPermissionScreens(UpdatePermissionModel source, Permission destination, ResolutionContext context)
        {
            if (destination.PermissionScreens != null)
            {
                foreach (var permissionScreen in destination.PermissionScreens)
                {
                    var getScreen = source.Screens.FirstOrDefault(s => s.ScreenId == permissionScreen.ScreenId);

                    if (getScreen != null && getScreen.Actions != null)
                    {
                        permissionScreen.PermissionScreenActions = new List<PermissionScreenAction>();
                        permissionScreen.PermissionScreenActions.AddRange(getScreen.Actions.Select(a => new PermissionScreenAction
                        {
                            ActionCode = a
                        }).ToList());
                    }
                }
            }
        }
    }
}
