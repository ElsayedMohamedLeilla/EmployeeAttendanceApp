using Dawem.Contract.BusinessLogic.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Permissions.PermissionLogs;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Permissions.PermissionLogs;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Permissions
{
    public class PermissionLogBL : IPermissionLogBL
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public PermissionLogBL(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<GetPermissionLogsResponse> Get(GetPermissionLogsCriteria model)
        {
            var permissionLogRepository = repositoryManager.PermissionLogRepository;
            var query = permissionLogRepository.GetAsQueryable(model);

            #region paging
            int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
            int take = PagingHelper.Take(model.PageSize);
            #region sorting
            var queryOrdered = permissionLogRepository.OrderBy(query, nameof(Permission.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var permissionLogsList = await queryPaged.Select(pl => new GetPermissionLogsResponseModel
            {
                Id = pl.Id,
                UserName = pl.User.Name,
                ScreenName = TranslationHelper.GetTranslation(pl.ScreenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang),
                IsActive = pl.IsActive,
            }).ToListAsync();

            return new GetPermissionLogsResponse
            {
                PermissionLogs = permissionLogsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetPermissionLogInfoResponseModel> GetInfo(int permissionLogId)
        {
            var permissionLog = await repositoryManager.PermissionLogRepository.Get(e => e.Id == permissionLogId && !e.IsDeleted)
                .Select(pl => new GetPermissionLogInfoResponseModel
                {
                    UserName = pl.User.Name,
                    ScreenName = TranslationHelper.GetTranslation(pl.ScreenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang),
                    ActionName = TranslationHelper.GetTranslation(pl.ActionCode.ToString(), requestInfo.Lang),
                    Date = pl.Date,
                    IsActive = pl.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permissionLog;
        }
    }

}

