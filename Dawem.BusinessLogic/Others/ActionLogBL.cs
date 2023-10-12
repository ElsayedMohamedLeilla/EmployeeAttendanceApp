using AutoMapper;
using SmartBusinessERP.BusinessLogic.Others.Contract;
using SmartBusinessERP.Data;
using SmartBusinessERP.Data.UnitOfWork;
using SmartBusinessERP.Domain.Entities.Core;
using SmartBusinessERP.Enums;
using SmartBusinessERP.Helpers;
using SmartBusinessERP.Models.Context;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Others;
using SmartBusinessERP.Models.Dtos.Others;
using SmartBusinessERP.Models.DtosMappers.Others;
using SmartBusinessERP.Models.Response;
using SmartBusinessERP.Models.Response.Core;
using SmartBusinessERP.Models.Response.Others;
using SmartBusinessERP.Repository.Others.Conract;

namespace SmartBusinessERP.BusinessLogic.Others
{
    public class ActionLogBL : IActionLogBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IActionLogRepository actionLogRepository;
        private readonly IMapper mapper;
        private readonly RequestHeaderContext userContext;


        public ActionLogBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IActionLogRepository _actionLogRepository, RequestHeaderContext _userContext,IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            actionLogRepository = _actionLogRepository;
            mapper = _mapper;
            userContext = _userContext;
        }

        public BaseResponseT<ActionLogDTO> GetById(int Id)
        {
            BaseResponseT<ActionLogDTO> response = new();
            try
            {
                var result = actionLogRepository.Get(x => x.Id == Id).FirstOrDefault();
                if (result == null)
                {
                    TranslationHelper.SetResponseMessages(response, "NoDataFound", "No Data Found", "");
                    response.Status = ResponseStatus.Error;
                }
                else
                {
                    response.Result = ActionLogDTOMapper.Map(result);
                    response.Status = ResponseStatus.Success;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Error;
                response.Exception = ex; response.Message = ex.Message;
            }
            return response;
        }
        public GetActionLogsResponse Get(GetActionLogsCriteria criteria)
        {
            var result = new GetActionLogsResponse();

            try
            {

                var query = actionLogRepository.GetAsQueryable(criteria);
                var queryOrdered = actionLogRepository.OrderBy(query, "Id", "desc");

                #region paging

                var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
                var take = PagingHelper.Take(criteria.PageSize);

                var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

                #endregion

                var actionLogs = queryPaged.ToList();
                ActionLogDTOMapper.InitActionLogContext(userContext);
                result.ActionLogs = ActionLogDTOMapper.Map(actionLogs);
                result.Status = ResponseStatus.Success;
                result.TotalCount = query.Count();
            }
            catch (Exception ex)
            {
                result.Exception = ex; result.Message = ex.Message;
                result.Status = ResponseStatus.Error;
            }

            return result;
        }
        public async Task<GetActionLogInfoResponse> GetInfo(GetActionLogInfoCriteria criteria)
        {

            GetActionLogInfoResponse actionLogSearchResult = new()
            {
                Status = ResponseStatus.Success
            };
            try
            {
                var actionLog = await actionLogRepository.GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, "Branch,User");

                if (actionLog != null)
                {
                    ActionLogDTOMapper.InitActionLogContext(userContext);
                    var actionLogInfo = ActionLogDTOMapper.MapInfo(actionLog);
                    actionLogSearchResult.ActionLogInfo = actionLogInfo;
                    actionLogSearchResult.Status = ResponseStatus.Success;
                }
                else
                {
                    actionLogSearchResult.Status = ResponseStatus.ValidationError;
                    TranslationHelper
                    .SetResponseMessages
                        (actionLogSearchResult, "ActionLogNotFound!",
                        "ActionLog Not Found !", lang: userContext.Lang);
                }


            }
            catch (Exception ex)
            {
                actionLogSearchResult.Exception = ex;
                actionLogSearchResult.Status = ResponseStatus.Error;
            }

            return actionLogSearchResult;

        }
        public async Task<BaseResponseT<bool>> Create(CreateActionLogModel model)
        {
            var response = new BaseResponseT<bool>();
            try
            {
                if (model.ActionType > 0 && model.ActionPlace > 0)
                {
                    unitOfWork.CreateTransaction();

                    var actionLog = new ActionLog()
                    {
                        ActionType = model.ActionType,
                        ActionPlace = model.ActionPlace,
                        Date = DateTime.Now,
                        BranchId = userContext?.BranchId ?? 0,
                        UserId = userContext?.UserId ?? 0,
                        ResponseStatus = model.ResponseStatus
                    };


                    actionLogRepository.Insert(actionLog);
                    await unitOfWork.SaveAsync();
                    unitOfWork.Commit();
                }
                
                response.Status = ResponseStatus.Success;

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                TranslationHelper.SetException(response, ex);
            }
            return response;
        }
    }

}

