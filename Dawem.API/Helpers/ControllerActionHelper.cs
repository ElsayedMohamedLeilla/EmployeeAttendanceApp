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
            DawemAdminApplicationAction? method = null;

            var allScreenCodes = type == AuthenticationType.AdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(DawemAdminApplicationScreenCode)).Cast<int>().ToList();


            screen = allScreenCodes.FirstOrDefault(s => EnumHelper.GetScreenName(s, type) + LeillaKeys.Controller == controllerName);

            if (screen != null)
            {
                if (actionName.Contains(LeillaKeys.Create))
                {
                    method = DawemAdminApplicationAction.AdditionAction;
                }
                else if (actionName.Contains(LeillaKeys.Update))
                {
                    method = DawemAdminApplicationAction.EditAction;
                }
                else if (actionName.Contains(LeillaKeys.Delete))
                {
                    method = DawemAdminApplicationAction.DeletionAction;
                }
                else if (actionName.Contains(LeillaKeys.Get))
                {
                    method = DawemAdminApplicationAction.ViewingAction;
                }
                else if (actionName.Contains(LeillaKeys.Accept))
                {
                    method = DawemAdminApplicationAction.AcceptAction;
                }
                else if (actionName.Contains(LeillaKeys.Reject))
                {
                    method = DawemAdminApplicationAction.RejectAction;
                }
                else if (actionName.Contains(LeillaKeys.Enable))
                {
                    method = DawemAdminApplicationAction.EnableAction;
                }
                else if (actionName.Contains(LeillaKeys.Disable))
                {
                    method = DawemAdminApplicationAction.DisableAction;
                }
            }

            response.Screen = screen;
            response.Method = method;
            return response;
        }
        public static GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions(RequestInfo requestInfo)
        {
            var response = new GetAllScreensWithAvailableActionsResponse();

            var allScreenCodes = requestInfo.Type == AuthenticationType.AdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(DawemAdminApplicationScreenCode)).Cast<int>().ToList();

            foreach (var tempScreenCode in allScreenCodes)
            {
                dynamic screenCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)tempScreenCode:
                    (DawemAdminApplicationScreenCode)tempScreenCode;

                var screenNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

                var screensWithAvailableActionsDTO = new ScreenWithAvailableActionsDTO
                {
                    ScreenCode = (int)screenCode,
                    ScreenName = TranslationHelper.GetTranslation(screenCode.ToString() + screenNameSuffix, requestInfo.Lang),
                    AvailableActions = GetScreenAvailableActions(screenCode.ToString(), requestInfo.Type)
                };

                response.Screens.Add(screensWithAvailableActionsDTO);
            }
            return response;
        }
        public static List<DawemAdminApplicationAction> GetScreenAvailableActions(string screenName, AuthenticationType type)
        {
            var actions = new List<DawemAdminApplicationAction>();

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
                        actions.Add(DawemAdminApplicationAction.AdditionAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Update))
                    {
                        actions.Add(DawemAdminApplicationAction.EditAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Delete))
                    {
                        actions.Add(DawemAdminApplicationAction.DeletionAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Get))
                    {
                        actions.Add(DawemAdminApplicationAction.ViewingAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Accept))
                    {
                        actions.Add(DawemAdminApplicationAction.AcceptAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Reject))
                    {
                        actions.Add(DawemAdminApplicationAction.RejectAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Enable))
                    {
                        actions.Add(DawemAdminApplicationAction.EnableAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Disable))
                    {
                        actions.Add(DawemAdminApplicationAction.DisableAction);
                    }
                }
            }

            return actions.Distinct().Order().ToList();
        }
        public static GetScreensForDropDownResponse GetAllScreens(RequestInfo requestInfo)
        {
            var response = new GetScreensForDropDownResponse();

            var allScreenCodes = requestInfo.Type == AuthenticationType.AdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(DawemAdminApplicationScreenCode)).Cast<int>().ToList();

            foreach (var tempScreenCode in allScreenCodes)
            {
                dynamic screenCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)tempScreenCode :
                    (DawemAdminApplicationScreenCode)tempScreenCode;

                var screenNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

                var screensWithAvailableActionsDTO = new BaseGetForDropDownResponseModel
                {
                    Id = (int)screenCode,
                    Name = TranslationHelper.GetTranslation(screenCode.ToString() + screenNameSuffix, requestInfo.Lang)
                };

                response.Screens.Add(screensWithAvailableActionsDTO);
            }
            return response;
        }
        public static GetActionsForDropDownResponse GetAllActions(RequestInfo requestInfo)
        {
            var response = new GetActionsForDropDownResponse();

            var allActionCodes = requestInfo.Type == AuthenticationType.AdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationAction)).Cast<int>().ToList() :
                Enum.GetValues(typeof(DawemAdminApplicationAction)).Cast<int>().ToList();

            foreach (var tempActionCode in allActionCodes)
            {
                dynamic actionCode = requestInfo.Type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationAction)tempActionCode :
                    (DawemAdminApplicationAction)tempActionCode;

                var actionNameSuffix = requestInfo.Type == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelAction :
                    LeillaKeys.DawemAction;

                var actionsWithAvailableActionsDTO = new BaseGetForDropDownResponseModel
                {
                    Id = (int)actionCode,
                    Name = TranslationHelper.GetTranslation(actionCode.ToString() + actionNameSuffix, requestInfo.Lang)
                };

                response.Actions.Add(actionsWithAvailableActionsDTO);
            }
            return response;
        }
    }
}

