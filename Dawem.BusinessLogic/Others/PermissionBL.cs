using Dawem.Contract.BusinessLogic.Others;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Configration;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Reflection;

namespace Dawem.BusinessLogic.Others
{
    public class PermissionBL : IPermissionBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;

        public PermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager, RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
            requestInfo = _requestHeaderContext;
        }
        public GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions()
        {
            var response = new GetAllScreensWithAvailableActionsResponse();
            var allScreenCodes = Enum.GetValues(typeof(ApplicationScreenCode)).Cast<ApplicationScreenCode>().ToList();

            foreach (var screenCode in allScreenCodes)
            {
                var screensWithAvailableActionsDTO = new ScreensWithAvailableActionsDTO
                {
                    ScreenCode = screenCode,
                    ScreenName = TranslationHelper.GetTranslation(screenCode.ToString(), requestInfo.Lang),
                    AvailableActions = GetScreenAvailableActions(screenCode)
                };

                response.Screens.Add(screensWithAvailableActionsDTO);
            }

            return response;
        }
        public static List<ApplicationAction> GetScreenAvailableActions(ApplicationScreenCode screen)
        {
            var response = new List<ApplicationAction>();

            //var response = new List<ApplicationAction>();

            Assembly asm //= Assembly.GetExecutingAssembly();
             = Assembly.GetAssembly(typeof(ControllerBase));

            var controllerType = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type)) //filter controllers
                .FirstOrDefault(c => c.Name == screen.ToString());

            var controllerTyggpe = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type));


            var fdff = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type)) //filter controllers
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));

            if (controllerType != null)
            {
                var methodNames =
                    controllerType.GetMethods()
                    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                    .Select(m => m.Name)
                    .ToList();

                foreach (var methodName in methodNames)
                {
                    if (methodName.Contains("Create"))
                    {
                        response.Add(ApplicationAction.AdditionAction);
                    }
                    else if (methodName.Contains("Update"))
                    {
                        response.Add(ApplicationAction.EditAction);
                    }
                    else if (methodName.Contains("Delete"))
                    {
                        response.Add(ApplicationAction.DeletionAction);
                    }
                    else if (methodName.Contains("Get"))
                    {
                        response.Add(ApplicationAction.ViewingAction);
                    }
                    else if (methodName.Contains("Accept"))
                    {
                        response.Add(ApplicationAction.AcceptAction);
                    }
                    else if (methodName.Contains("Reject"))
                    {
                        response.Add(ApplicationAction.RejectAction);
                    }
                    else if (methodName.Contains("Enable"))
                    {
                        response.Add(ApplicationAction.EnableAction);
                    }
                    else if (methodName.Contains("Disable"))
                    {
                        response.Add(ApplicationAction.DisableAction);
                    }
                }
            }










            /*

            try
            {
                switch (screen)
                {
                    case ApplicationScreenCode.AttendancesScreen:

                        response = new List<ApplicationAction>() { ApplicationAction.AdditionAction, ApplicationAction.ViewingAction, ApplicationAction.DeletionAction };
                        break;

                    case ApplicationScreenCode.DashboardScreen:

                        response = new List<ApplicationAction>() { ApplicationAction.ViewingAction };
                        break;

                    case ApplicationScreenCode.AssignmentTypesScreen:
                    case ApplicationScreenCode.PermissionTypesScreen:
                    case ApplicationScreenCode.JustificationTypesScreen:
                    case ApplicationScreenCode.TaskTypesScreen:
                    case ApplicationScreenCode.HolidayTypesScreen:
                    case ApplicationScreenCode.HolidaysScreen:
                    case ApplicationScreenCode.VacationTypesScreen:
                    case ApplicationScreenCode.FingerprintDevicesScreen:
                    case ApplicationScreenCode.ZonesScreen:
                    case ApplicationScreenCode.GroupsScreen:
                    case ApplicationScreenCode.DepartmentsScreen:

                        response = new List<ApplicationAction>() { ApplicationAction.AdditionAction, ApplicationAction.EditAction, ApplicationAction.ViewingAction, ApplicationAction.DeletionAction };
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
            }*/

            return response;
        }
        public async Task<bool> CheckUserPermission(CheckUserPermissionModel model)
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var permissionScreenActionRepository = repositoryManager.PermissionScreenActionRepository;
            var userRepository = repositoryManager.UserRepository;

            var getUserRoles = await userRepository.Get(u => !u.IsDeleted && u.Id == model.UserId)
                .Select(u => new { UserRoles = u.UserRoles.Select(ur => ur.RoleId) })
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            var roles = getUserRoles.UserRoles.Select(roleId => roleId).ToList();

            if (roles != null && roles.Count > 0)
            {
                var checkIfHasPermission = await permissionScreenActionRepository
                    .Get(p => !p.IsDeleted && !p.PermissionScreen.IsDeleted && !p.PermissionScreen.Permission.IsDeleted &&
                    p.PermissionScreen.Permission.CompanyId == requestInfo.CompanyId &&
                    p.PermissionScreen.ScreenCode == model.Screen
                     && p.Action == model.Action && roles.Contains(p.PermissionScreen.Permission.RoleId))
                    .AnyAsync();

                if (!checkIfHasPermission)
                {
                    var checkIfHasAnyPermissino = await permissionRepository
                        .Get(p => !p.IsDeleted && roles.Contains(p.RoleId)).AnyAsync();
                    if (checkIfHasAnyPermissino)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

}

