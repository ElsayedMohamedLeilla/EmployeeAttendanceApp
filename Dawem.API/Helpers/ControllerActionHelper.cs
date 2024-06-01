using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.Response;
using Dawem.Models.Response.Dawem.Attendances.FingerprintDevices;
using Dawem.Models.Response.Dawem.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Dawem.API.Helpers
{
    public static class ControllerActionHelper
    {
        public static MapControllerAndActionResponse MapControllerAndAction(string controllerName, string actionName, AuthenticationType type)
        {
            var response = new MapControllerAndActionResponse();

            int? screen = null;
            ApplicationActionCode? method = null;

            var allScreenCodes = type == AuthenticationType.AdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(DawemAdminApplicationScreenCode)).Cast<int>().ToList();


            screen = allScreenCodes.FirstOrDefault(s => EnumHelper.GetScreenName(s, type) + LeillaKeys.Controller == controllerName);

            if (screen != null)
            {
                if (actionName.Contains(LeillaKeys.Create))
                {
                    method = ApplicationActionCode.AdditionAction;
                }
                else if (actionName.Contains(LeillaKeys.Update))
                {
                    method = ApplicationActionCode.EditAction;
                }
                else if (actionName.Contains(LeillaKeys.Delete))
                {
                    method = ApplicationActionCode.DeletionAction;
                }
                else if (actionName.Contains(LeillaKeys.Get))
                {
                    method = ApplicationActionCode.ViewingAction;
                }
                else if (actionName.Contains(LeillaKeys.Accept))
                {
                    method = ApplicationActionCode.AcceptAction;
                }
                else if (actionName.Contains(LeillaKeys.Reject))
                {
                    method = ApplicationActionCode.RejectAction;
                }
                else if (actionName.Contains(LeillaKeys.Enable))
                {
                    method = ApplicationActionCode.EnableAction;
                }
                else if (actionName.Contains(LeillaKeys.Disable))
                {
                    method = ApplicationActionCode.DisableAction;
                }
            }

            response.ScreenCode = screen;
            response.ActionCode = method;
            return response;
        }
        public static OldGetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions(RequestInfo requestInfo, bool? IsForMenu = false)
        {
            var response = new OldGetAllScreensWithAvailableActionsResponse();

            var allScreenCodes = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(DawemAdminApplicationScreenCode)).Cast<int>().ToList();

            foreach (var tempScreenCode in allScreenCodes)
            {
                dynamic screenCode = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)tempScreenCode :
                    (DawemAdminApplicationScreenCode)tempScreenCode;

                if (IsForMenu.HasValue && IsForMenu.Value && requestInfo.AuthenticationType == AuthenticationType.DawemAdmin)
                {
                    var screenName = screenCode.ToString();
                    if (screenName.StartsWith(LeillaKeys.Employee) && screenName.Length > LeillaKeys.Employee.Length)
                        continue;
                }

                var screenNameSuffix = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

                var screensWithAvailableActionsDTO = new OldScreenWithAvailableActionsDTO
                {
                    ScreenCode = (int)screenCode,
                    ScreenName = TranslationHelper.GetTranslation(screenCode.ToString() + screenNameSuffix, requestInfo.Lang),
                    AvailableActions = GetScreenAvailableActions(screenCode.ToString(), requestInfo.AuthenticationType)
                };

                response.Screens.Add(screensWithAvailableActionsDTO);
            }
            return response;
        }
        public static List<ApplicationActionCode> GetScreenAvailableActions(string screenName, AuthenticationType type)
        {
            var actions = new List<ApplicationActionCode>();

            var assembly = Assembly.GetExecutingAssembly();
            var controllerType = type == AuthenticationType.AdminPanel ?
                assembly.GetTypes()
                .Where(type => typeof(AdminPanelControllerBase).IsAssignableFrom(type))
                .FirstOrDefault(c => c.Name == screenName + LeillaKeys.Controller) :
                assembly.GetTypes()
                .Where(type => typeof(DawemControllerBase).IsAssignableFrom(type))
                .FirstOrDefault(c => c.Name == screenName + LeillaKeys.Controller);

            if (controllerType != null)
            {
                var methodNames =
                    controllerType.GetMethods()
                    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)))
                    .Select(m => m.Name)
                    .ToList();

                foreach (var methodName in methodNames)
                {
                    if (methodName.Contains(LeillaKeys.Create))
                    {
                        actions.Add(ApplicationActionCode.AdditionAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Update))
                    {
                        actions.Add(ApplicationActionCode.EditAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Delete))
                    {
                        actions.Add(ApplicationActionCode.DeletionAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Get))
                    {
                        actions.Add(ApplicationActionCode.ViewingAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Accept))
                    {
                        actions.Add(ApplicationActionCode.AcceptAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Reject))
                    {
                        actions.Add(ApplicationActionCode.RejectAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Enable))
                    {
                        actions.Add(ApplicationActionCode.EnableAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Disable))
                    {
                        actions.Add(ApplicationActionCode.DisableAction);
                    }
                }
            }

            return actions.Distinct().Order().ToList();
        }
    }
}

