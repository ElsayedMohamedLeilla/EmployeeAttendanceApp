using Dawem.Contract.BusinessLogic.Others;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Ohters;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Others;
using Dawem.Models.DtosMappers.Others;
using Dawem.Models.Exceptions;
using Dawem.Models.ResponseModels;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Others
{
    public class ActionLogBL : IActionLogBL
    {

        private IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo userContext;


        public ActionLogBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager, RequestInfo _userContext
            )
        {
            unitOfWork = _unitOfWork;
            repositoryManager = _repositoryManager;
            userContext = _userContext;
        }

        public async Task<ActionLogDTO> GetById(int Id)
        {
            var actionLog = await repositoryManager.ActionLogRepository.GetByIdAsync(Id) ??
                throw new BusinessValidationException(LeillaKeys.NoDataFound);

            var response = ActionLogDTOMapper.Map(actionLog);

            return response;
        }
        public async Task<GetActionLogsResponseModel> Get(GetActionLogsCriteria criteria)
        {

            var query = repositoryManager.ActionLogRepository.GetAsQueryable(criteria);
            var queryOrdered = repositoryManager.ActionLogRepository.OrderBy(query, "Id", "desc");

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
                TotalCount = await query.CountAsync()
            };

            return response;
        }
        public async Task<ActionLogInfo> GetInfo(GetActionLogInfoCriteria criteria)
        {
            var actionLog = await repositoryManager.ActionLogRepository
                .GetEntityByConditionWithTrackingAsync(u => u.Id == criteria.Id, "Branch,User")
                ?? throw new BusinessValidationException(LeillaKeys.ActionLogNotFound);

            ActionLogDTOMapper.InitActionLogContext(userContext);
            var actionLogInfo = ActionLogDTOMapper.MapInfo(actionLog);

            return actionLogInfo;

        }
        public async Task<bool> Create(CreateActionLogModel model)
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


                repositoryManager.ActionLogRepository.Insert(actionLog);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
            }

            return true;
        }
    }

}

