using Dawem.Contract.BusinessLogic.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Others;
using Dawem.Models.DtosMappers.Others;
using Dawem.Models.Exceptions;
using Dawem.Models.ResponseModels;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Permissions
{
    public class ActionLogBL : IPermissionLogBL
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
            var actionLog = await repositoryManager.ScreenPermissionLogRepository.GetByIdAsync(Id) ??
                throw new BusinessValidationException(LeillaKeys.NoDataFound);

            var response = ActionLogDTOMapper.Map(actionLog);

            return response;
        }
        public async Task<GetActionLogsResponseModel> Get(GetPermissionLogsCriteria criteria)
        {

            var query = repositoryManager.ScreenPermissionLogRepository.GetAsQueryable(criteria);
            var queryOrdered = repositoryManager.ScreenPermissionLogRepository.OrderBy(query, "Id", "desc");

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
            var actionLog = await repositoryManager.ScreenPermissionLogRepository
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

                var actionLog = new PermissionLog()
                {
                    ActionType = model.ActionType,
                    ScreenCode = model.ActionPlace,
                    Date = DateTime.Now,
                    CompanyId = userContext?.BranchId ?? 0,
                    UserId = userContext?.UserId ?? 0,
                    ResponseStatus = model.ResponseStatus
                };


                repositoryManager.ScreenPermissionLogRepository.Insert(actionLog);
                await unitOfWork.SaveAsync();
                unitOfWork.Commit();
            }

            return true;
        }
    }

}

