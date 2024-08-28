using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Permissions;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.PermissionLogs;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Permissions.PermissionLogs;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Permissions
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

            var screenNameSuffix = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

            #region paging

            int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
            int take = PagingHelper.Take(model.PageSize);

            #region sorting
            var queryOrdered = permissionLogRepository.OrderBy(query, nameof(Permission.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = model.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var permissionLogsList = await queryPaged.Select(pl => new GetPermissionLogsResponseModel
            {
                Id = pl.Id,
                UserName = pl.User.Name,
                ScreenName = pl.Screen.MenuItemNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                ActionName = TranslationHelper.GetTranslation(pl.ActionCode.ToString(), requestInfo.Lang)
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
            var screenNameSuffix = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

            var permissionLog = await repositoryManager.PermissionLogRepository.
                Get(permissionLog => permissionLog.Id == permissionLogId && !permissionLog.IsDeleted &&
                ((requestInfo.CompanyId > 0 && permissionLog.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permissionLog.CompanyId == null)) &&
                permissionLog.Type == requestInfo.AuthenticationType)
                .Select(pl => new GetPermissionLogInfoResponseModel
                {
                    UserName = pl.User.Name,
                    ScreenName = pl.Screen.MenuItemNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    ActionName = TranslationHelper.GetTranslation(pl.ActionCode.ToString(), requestInfo.Lang),
                    DateAndTime = pl.Company.Country.TimeZoneToUTC != null ?
                    pl.DateUTC.AddHours((double)pl.Company.Country.TimeZoneToUTC.Value) :
                    pl.DateUTC.AddHours(2),
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permissionLog;
        }
    }

}

