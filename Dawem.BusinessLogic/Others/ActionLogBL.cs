using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.General;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others;
using Dawem.Models.Exceptions;
using Dawem.Models.Response;
using Dawem.Models.Response.Others;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using SmartBusinessERP.BusinessLogic.Others.Contract;
using SmartBusinessERP.Models.Criteria.Core;
using SmartBusinessERP.Models.Criteria.Others;
using SmartBusinessERP.Models.DtosMappers.Others;

namespace SmartBusinessERP.BusinessLogic.Others
{
    public class ActionLogBL : IActionLogBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IActionLogRepository actionLogRepository;
        private readonly RequestHeaderContext userContext;


        public ActionLogBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IActionLogRepository _actionLogRepository, RequestHeaderContext _userContext
            )
        {
            unitOfWork = _unitOfWork;
            actionLogRepository = _actionLogRepository;
            userContext = _userContext;
        }

        public async Task<ActionLogDTO> GetById(int Id)
        {
            var actionLog = await actionLogRepository.GetByIdAsync(Id) ??
                throw new BusinessValidationErrorException(DawemKeys.NoDataFound);

            var response = ActionLogDTOMapper.Map(actionLog);

            return response;
        }
        public async Task<GetActionLogsResponseModel> Get(GetActionLogsCriteria criteria)
        {

            var query = actionLogRepository.GetAsQueryable(criteria);
            var queryOrdered = actionLogRepository.OrderBy(query, "Id", "desc");

            #region paging

            var skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            var take = PagingHelper.Take(criteria.PageSize);

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            var tempActionLogs = await queryPaged.ToListAsync();
            ActionLogDTOMapper.InitActionLogContext(userContext);
            var actionLogs = ActionLogDTOMapper.Map(tempActionLogs);

            var response = new GetActionLogsResponseModel
            {
                ActionLogs = actionLogs,
                TotalCount = query.Count()
            };

            return response;
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

