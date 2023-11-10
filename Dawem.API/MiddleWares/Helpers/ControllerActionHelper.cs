using Dawem.API.Controllers.Employees;
using Dawem.API.Controllers.Others;
using Dawem.API.Controllers.Provider;
using Dawem.API.Controllers.UserManagement;
using Dawem.Enums.Generals;
using Dawem.Models.Response.Others;

namespace Dawem.API.MiddleWares.Helpers
{
    public static class ControllerActionHelper
    {
        public static MapControllerAndActionResponse MapControllerAndAction(string controllerName, string actionName)
        {
            var response = new MapControllerAndActionResponse();
            response.Status = ResponseStatus.Success;

            ApplicationScreenType? screen = null;
            ApiMethod? method = null;
            try
            {
                switch (controllerName)
                {
                    case nameof(AuthenticationController):

                        screen = ApplicationScreenType.LogInScreen;
                        if (actionName == nameof(AuthenticationController.SignIn))
                        {
                            method = ApiMethod.LogIn;
                        }

                        break;


                    case nameof(UserController):

                        screen = ApplicationScreenType.UsersScreen;
                        if (actionName == nameof(UserController.GetInfo) || actionName == nameof(UserController.Get))
                        {
                            method = ApiMethod.View;
                        }
                        else if (actionName == nameof(UserController.Create))
                        {
                            method = ApiMethod.Add;
                        }
                        else if (actionName == nameof(UserController.Update))
                        {
                            method = ApiMethod.Update;
                        }
                        else if (actionName == nameof(UserController.Delete))
                        {
                            method = ApiMethod.Delete;
                        }

                        break;

                    case nameof(ActionLogController):

                        screen = ApplicationScreenType.ActionsLogsScreen;
                        if (actionName == nameof(ActionLogController.GetInfo) || actionName == nameof(ActionLogController.Get))
                        {
                            method = ApiMethod.View;
                        }

                        break;


                    default:
                        break;
                }

                response.Screen = screen;
                response.Method = method;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Message = ex.Message;
            }

            return response;
        }

    }
}

