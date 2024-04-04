using Dawem.API.Areas.Dawem.Controllers;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.Response.Dawem.Others;
using Dawem.Translations;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Dawem.API.Helpers
{
    public static class ControllerActionHelper
    {
        public static MapControllerAndActionResponse MapControllerAndAction(string controllerName, string actionName, bool isAdminPanel)
        {
            var response = new MapControllerAndActionResponse();

            int? screen = null;
            ApplicationAction? method = null;

            var allScreenCodes = isAdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(ApplicationScreenCode)).Cast<int>().ToList();

            screen = allScreenCodes.FirstOrDefault(s => s.ToString() + LeillaKeys.Controller == controllerName);

            if (screen != null)
            {
                if (actionName.Contains(LeillaKeys.Create) || actionName.Contains(LeillaKeys.Send))
                {
                    method = ApplicationAction.AdditionAction;
                }
                else if (actionName.Contains(LeillaKeys.Update) || actionName.Contains(LeillaKeys.MarkAsRead) || actionName.Contains(AmgadKeys.MarkAsViewed))
                {
                    method = ApplicationAction.EditAction;
                }
                else if (actionName.Contains(LeillaKeys.Delete))
                {
                    method = ApplicationAction.DeletionAction;
                }
                else if (actionName.Contains(LeillaKeys.Get))
                {
                    method = ApplicationAction.ViewingAction;
                }
                else if (actionName.Contains(LeillaKeys.Accept))
                {
                    method = ApplicationAction.AcceptAction;
                }
                else if (actionName.Contains(LeillaKeys.Reject))
                {
                    method = ApplicationAction.RejectAction;
                }
                else if (actionName.Contains(LeillaKeys.Enable))
                {
                    method = ApplicationAction.EnableAction;
                }
                else if (actionName.Contains(LeillaKeys.Disable))
                {
                    method = ApplicationAction.DisableAction;
                }
            }

            response.Screen = screen;
            response.Method = method;
            return response;
        }
        public static GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions(RequestInfo requestInfo)
        {
            var response = new GetAllScreensWithAvailableActionsResponse();

            var allScreenCodes = requestInfo.IsAdminPanel ?
                Enum.GetValues(typeof(AdminPanelApplicationScreenCode)).Cast<int>().ToList() :
                Enum.GetValues(typeof(ApplicationScreenCode)).Cast<int>().ToList();

            foreach (var tempScreenCode in allScreenCodes)
            {
                dynamic screenCode = requestInfo.IsAdminPanel ?
                    (AdminPanelApplicationScreenCode)tempScreenCode:
                    (ApplicationScreenCode)tempScreenCode;

                var screensWithAvailableActionsDTO = new ScreensWithAvailableActionsDTO
                {
                    ScreenCode = (int)screenCode,
                    ScreenName = TranslationHelper.GetTranslation(screenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang),
                    AvailableActions = GetScreenAvailableActions(screenCode.ToString(), requestInfo.IsAdminPanel)
                };

                response.Screens.Add(screensWithAvailableActionsDTO);
            }
            return response;
        }
        public static List<ApplicationAction> GetScreenAvailableActions(string screenName, bool isAdminPanel)
        {
            var actions = new List<ApplicationAction>();

            var assembly = Assembly.GetExecutingAssembly();
            var controllerType = isAdminPanel ?  
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
                        actions.Add(ApplicationAction.AdditionAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Update))
                    {
                        actions.Add(ApplicationAction.EditAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Delete))
                    {
                        actions.Add(ApplicationAction.DeletionAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Get))
                    {
                        actions.Add(ApplicationAction.ViewingAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Accept))
                    {
                        actions.Add(ApplicationAction.AcceptAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Reject))
                    {
                        actions.Add(ApplicationAction.RejectAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Enable))
                    {
                        actions.Add(ApplicationAction.EnableAction);
                    }
                    else if (methodName.Contains(LeillaKeys.Disable))
                    {
                        actions.Add(ApplicationAction.DisableAction);
                    }
                }
            }

            return actions.Distinct().Order().ToList();
        }
    }
}

