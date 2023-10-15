using Dawem.Contract.BusinessLogic.Others;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.General;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Response.Others;

namespace Dawem.BusinessLogic.Others
{
    public class UserScreenActionPermissionBL : IUserScreenActionPermissionBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestHeaderContext requestHeaderContext;


        public UserScreenActionPermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager, RequestHeaderContext _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
            requestHeaderContext = _requestHeaderContext;
        }


        public GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions()
        {
            var response = new GetAllScreensWithAvailableActionsResponse();
            response.Status = ResponseStatus.Success;
            try
            {
                var ApplicationScreenTypes = Enum.GetValues(typeof(ApplicationScreenType)).Cast<ApplicationScreenType>().ToList();

                foreach (var erpScreen in ApplicationScreenTypes)
                {
                    var screensWithAvailableActionsDTO = new ScreensWithAvailableActionsDTO();

                    screensWithAvailableActionsDTO.Screen = erpScreen;
                    screensWithAvailableActionsDTO.AvailableActions = GetScreenAvailableActions(erpScreen);

                    response.Screens.Add(screensWithAvailableActionsDTO);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                response.Status = ResponseStatus.Error;
            }

            return response;
        }


        public List<ApiMethod> GetScreenAvailableActions(ApplicationScreenType screen)
        {
            var response = new List<ApiMethod>();
            try
            {
                switch (screen)
                {
                    case ApplicationScreenType.CompaniesScreen:
                        response = new List<ApiMethod>() { ApiMethod.View };
                        break;
                    case ApplicationScreenType.BranchesScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ApplicationScreenType.UsersScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ApplicationScreenType.PaymentMethodsScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ApplicationScreenType.StoresScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ApplicationScreenType.UnitsScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ApplicationScreenType.LogInScreen:
                        response = new List<ApiMethod>() { ApiMethod.LogIn };
                        break;
                    case ApplicationScreenType.AccountsScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ApplicationScreenType.ActionsLogsScreen:
                        response = new List<ApiMethod>() { ApiMethod.View };
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
            }

            return response;
        }



        //public async Task<BaseResponse> CheckUserPermission(CheckUserPermissionModel model)
        //{
        //    var result = new BaseResponse();

        //    try
        //    {



        //        AccountInfoController

        //        var query = actionLogRepository.GetAsQueryable(criteria,"Branch");
        //        var queryOrdered = actionLogRepository.OrderBy(query, "Id", "desc");

        //        #region paging

        //        var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
        //        var take = PagingHelper.Take(criteria.PageSize);

        //        var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

        //        #endregion

        //        var actionLogs = queryPaged.ToList();
        //        ActionLogDTOMapper.InitActionLogContext(userContext);
        //        result.ActionLogs = ActionLogDTOMapper.Map(actionLogs);
        //        result.Status = ResponseStatus.Success;
        //        result.TotalCount = await query.CountAsync()
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Exception = ex; result.Message = ex.Message;
        //        result.Status = ResponseStatus.Error;
        //    }

        //    return result;
        //}
    }

}

