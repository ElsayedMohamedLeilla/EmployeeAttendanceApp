using Dawem.Enums.General;

namespace SmartBusinessERP.API.MiddleWares.Helpers
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
                    case nameof(AccountController):

                        screen = ApplicationScreenType.LogInScreen;
                        if (actionName == nameof(AccountController.SignIn))
                        {
                            method = ApiMethod.LogIn;
                        }

                        break;

                    case nameof(UnitController):

                        screen = ApplicationScreenType.UnitsScreen;
                        if (actionName == nameof(UnitController.GetById) || actionName == nameof(UnitController.GetInfo) || actionName == nameof(UnitController.Get))
                        {
                            method = ApiMethod.View;
                        }
                        else if (actionName == nameof(UnitController.Create))
                        {
                            method = ApiMethod.Add;
                        }
                        else if (actionName == nameof(UnitController.Update))
                        {
                            method = ApiMethod.Update;
                        }
                        else if (actionName == nameof(UnitController.Delete))
                        {
                            method = ApiMethod.Delete;
                        }

                        break;

                    case nameof(PaymentMethodController):

                        screen = ApplicationScreenType.PaymentMethodsScreen;
                        if (actionName == nameof(PaymentMethodController.GetById) || actionName == nameof(PaymentMethodController.GetInfo) || actionName == nameof(PaymentMethodController.Get))
                        {
                            method = ApiMethod.View;
                        }
                        else if (actionName == nameof(PaymentMethodController.Create))
                        {
                            method = ApiMethod.Add;
                        }
                        else if (actionName == nameof(PaymentMethodController.Update))
                        {
                            method = ApiMethod.Update;
                        }
                        else if (actionName == nameof(PaymentMethodController.Delete))
                        {
                            method = ApiMethod.Delete;
                        }

                        break;


                    case nameof(StoreController):

                        screen = ApplicationScreenType.StoresScreen;
                        if (actionName == nameof(StoreController.GetById) || actionName == nameof(StoreController.GetInfo) || actionName == nameof(StoreController.Get))
                        {
                            method = ApiMethod.View;
                        }
                        else if (actionName == nameof(StoreController.Create))
                        {
                            method = ApiMethod.Add;
                        }
                        else if (actionName == nameof(StoreController.Update))
                        {
                            method = ApiMethod.Update;
                        }
                        else if (actionName == nameof(StoreController.Delete))
                        {
                            method = ApiMethod.Delete;
                        }

                        break;
                    case nameof(UserController):

                        screen = ApplicationScreenType.UsersScreen;
                        if (actionName == nameof(UserController.GetInfo) || actionName == nameof(StoreController.Get))
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
                        else if (actionName == nameof(UserController.DeleteUser))
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

