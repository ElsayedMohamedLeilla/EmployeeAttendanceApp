using AutoMapper;
using SmartBusinessERP.BusinessLogic.Others.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Dtos.Others;
using SmartBusinessERP.Models.Response.Others;
using SmartBusinessERP.Repository.Others.Conract;

namespace SmartBusinessERP.BusinessLogic.Others
{
    public class UserScreenActionPermissionBL : IUserScreenActionPermissionBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IActionLogRepository actionLogRepository;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;


        public UserScreenActionPermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IActionLogRepository _actionLogRepository, RequestHeaderContext _userContext, IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            actionLogRepository = _actionLogRepository;
            mapper = _mapper;
            userContext = _userContext;
        }


        public GetAllScreensWithAvailableActionsResponse GetAllScreensWithAvailableActions()
        {
            var response = new GetAllScreensWithAvailableActionsResponse();
            response.Status = ResponseStatus.Success;
            try
            {
                var ERPScreens = Enum.GetValues(typeof(ERPScreen)).Cast<ERPScreen>().ToList();

                foreach (var erpScreen in ERPScreens)
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


        public List<ApiMethod> GetScreenAvailableActions(ERPScreen screen)
        {
            var response = new List<ApiMethod>();
            try
            {
                switch (screen)
                {
                    case ERPScreen.CompaniesScreen:
                        response = new List<ApiMethod>() { ApiMethod.View };
                        break;
                    case ERPScreen.BranchesScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ERPScreen.UsersScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ERPScreen.PaymentMethodsScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ERPScreen.StoresScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ERPScreen.UnitsScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ERPScreen.LogInScreen:
                        response = new List<ApiMethod>() { ApiMethod.LogIn };
                        break;
                    case ERPScreen.AccountsScreen:
                        response = new List<ApiMethod>() { ApiMethod.Add, ApiMethod.Update, ApiMethod.View, ApiMethod.Delete };
                        break;
                    case ERPScreen.ActionsLogsScreen:
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
        //        result.TotalCount = query.Count();
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

