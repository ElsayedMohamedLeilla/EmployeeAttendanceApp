using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Response.Dawem.Others;
using Dawem.Models.Response.Dawem.Permissions.Permissions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Permissions
{
    public class PermissionBL : IPermissionBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IPermissionBLValidation permissionBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IScreenBLC screenBLC;
        private readonly IPermissionBLC permissionBLC;
        public PermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager, IScreenBLC _screenBLC,
            IMapper _mapper, IPermissionBLC _permissionBLC,
           RequestInfo _requestHeaderContext,
           IPermissionBLValidation _permissionBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            permissionBLValidation = _permissionBLValidation;
            mapper = _mapper;
            permissionBLC = _permissionBLC;
            screenBLC = _screenBLC;
        }
        public async Task<int> Create(CreatePermissionModel model)
        {
            await unitOfWork.CreateTransactionAsync();

            var permissionId = await permissionBLC.Create(model);

            await unitOfWork.CommitAsync();

            return permissionId;
        }
        public async Task<bool> Update(UpdatePermissionModel model)
        {
            #region Business Validation

            await permissionBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Permission

            var getPermission = await repositoryManager.PermissionRepository
                .GetWithTracking(permission => !permission.IsDeleted
                && permission.Id == model.Id
                && permission.Type == requestInfo.AuthenticationType &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)))
                .Include(p => p.PermissionScreens)
                .ThenInclude(ps => ps.PermissionScreenActions)
                .FirstOrDefaultAsync();

            if (getPermission != null)
            {
                getPermission.ForType = model.ForType;
                getPermission.ResponsibilityId = model.ResponsibilityId;
                getPermission.UserId = model.UserId;
                getPermission.IsActive = model.IsActive;
                getPermission.ModifiedDate = DateTime.Now;
                getPermission.ModifyUserId = requestInfo.UserId;

                #region Update Related Screens

                var dbPermissionScreens = getPermission.PermissionScreens;
                var modelPermissionScreens = model.Screens;

                var getAddedPermissionScreens = modelPermissionScreens
                    .Where(m => !dbPermissionScreens.Any(d => d.ScreenId == m.ScreenId))
                    .Select(m => new PermissionScreen
                    {
                        PermissionId = model.Id,
                        ScreenId = m.ScreenId,
                        PermissionScreenActions = m.Actions.Select(actionCode => new PermissionScreenAction
                        {
                            ActionCode = actionCode
                        }).ToList()
                    }).ToList();

                var getDeletedPermissionScreens = dbPermissionScreens
                    .Where(d => !modelPermissionScreens.Any(m => m.ScreenId == d.ScreenId))
                    .ToList();

                repositoryManager.PermissionScreenRepository.BulkInsert(getAddedPermissionScreens);
                repositoryManager.PermissionScreenRepository.BulkDeleteIfExist(getDeletedPermissionScreens);

                var getUpdatedPermissionScreens = modelPermissionScreens
                    .Where(ms => !getAddedPermissionScreens.Any(adds => adds.ScreenId == ms.ScreenId)
                    && !getDeletedPermissionScreens.Any(dele => dele.ScreenId == ms.ScreenId))
                    .ToList();


                #region Update Related Screen Actions

                if (getUpdatedPermissionScreens != null && getUpdatedPermissionScreens.Count > 0)
                {
                    foreach (var permissionScreen in getUpdatedPermissionScreens)
                    {
                        var dbPermissionScreen = dbPermissionScreens
                            .FirstOrDefault(ps => ps.ScreenId == permissionScreen.ScreenId);
                        var dbPermissionScreenActions = dbPermissionScreen.PermissionScreenActions;
                        var modelPermissionScreenActions = permissionScreen.Actions;

                        var getAddedPermissionScreenActions = modelPermissionScreenActions
                            .Where(actionCode => !dbPermissionScreenActions.Any(d => d.ActionCode == actionCode))
                            .Select(actionCode => new PermissionScreenAction
                            {
                                PermissionScreenId = dbPermissionScreen.Id,
                                ActionCode = actionCode,
                            }).ToList();

                        var getDeletedPermissionScreenActions = dbPermissionScreenActions
                            .Where(d => !modelPermissionScreenActions.Any(actionCode => actionCode == d.ActionCode))
                            .ToList();

                        repositoryManager.PermissionScreenActionRepository.BulkInsert(getAddedPermissionScreenActions);
                        repositoryManager.PermissionScreenActionRepository.BulkDeleteIfExist(getDeletedPermissionScreenActions);
                    }
                }

                #endregion

                #endregion

                await unitOfWork.SaveAsync();

                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            else
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            #endregion
        }
        public async Task<GetPermissionsResponse> Get(GetPermissionsCriteria criteria)
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var query = permissionRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = permissionRepository.OrderBy(query, nameof(Permission.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var permissionsList = await queryPaged.Select(p => new GetPermissionsResponseModel
            {
                Id = p.Id,
                Code = p.Code,
                ForType = p.ForType,
                ResponsibilityOrUserName = p.ResponsibilityId > 0 ? p.Responsibility.Name : p.User.Name,
                ForTypeName = TranslationHelper.GetTranslation(LeillaKeys.PermissionForType + p.ForType.ToString(), requestInfo.Lang),
                AllowedScreensCount = p.PermissionScreens.Count,
                IsActive = p.IsActive,
            }).ToListAsync();
            return new GetPermissionsResponse
            {
                Permissions = permissionsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetPermissionInfoResponseModel> GetInfo(int permissionId)
        {
            var screenNameSuffix = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

            var permission = await repositoryManager.PermissionRepository.Get(permission => permission.Id == permissionId
            && !permission.IsDeleted && ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.Type == requestInfo.AuthenticationType)
                .Select(p => new GetPermissionInfoResponseModel
                {
                    Code = p.Code,
                    ForType = p.ForType,
                    ForTypeName = TranslationHelper.GetTranslation(LeillaKeys.PermissionForType + p.ForType.ToString(), requestInfo.Lang),
                    ResponsibilityName = p.Responsibility != null ? p.Responsibility.Name : null,
                    UserName = p.User != null ? p.User.Name : null,
                    PermissionScreens = p.PermissionScreens
                    .OrderByDescending(e => e.Id)
                    .Take(5)
                    .Select(ps => new PermissionScreenResponseWithNamesModel
                    {
                        ScreenName = ps.Screen.MenuItemNameTranslations.FirstOrDefault(s => s.Language.ISO2 == requestInfo.Lang).Name,
                        PermissionScreenActions = ps.PermissionScreenActions.Select(psa => new PermissionScreenActionResponseWithNamesModel
                        {
                            ActionCode = psa.ActionCode,
                            ActionName = TranslationHelper.GetTranslation(psa.ActionCode.ToString(), requestInfo.Lang)
                        }).ToList()
                    }).ToList(),
                    IsActive = p.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permission;
        }
        public async Task<GetPermissionScreensResponse> GetPermissionScreens(GetPermissionScreensCriteria criteria)
        {
            #region Validation

            if (criteria.PermissionId <= 0)
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            #endregion

            var permissionScreenRepository = repositoryManager.PermissionScreenRepository;
            var query = permissionScreenRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = permissionScreenRepository.OrderBy(query, nameof(PermissionScreen.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var screenNameSuffix = requestInfo.AuthenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

            var permissionScreensList = await queryPaged.Select(ps => new GetPermissionScreenInfoModel
            {
                ScreenName = ps.Screen.MenuItemNameTranslations.FirstOrDefault(s => s.Language.ISO2 == requestInfo.Lang).Name,
                PermissionScreenActions = ps.PermissionScreenActions.Select(psa => new PermissionScreenActionResponseWithNamesModel
                {
                    ActionCode = psa.ActionCode,
                    ActionName = TranslationHelper.GetTranslation(psa.ActionCode.ToString(), requestInfo.Lang)
                }).ToList()
            }).ToListAsync();

            return new GetPermissionScreensResponse
            {
                PermissionScreens = permissionScreensList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetPermissionByIdResponseModel> GetById(int permissionId)
        {
            var permission = await repositoryManager.PermissionRepository
                .Get(permission => permission.Id == permissionId && !permission.IsDeleted && permission.IsActive &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.Type == requestInfo.AuthenticationType)
                .Select(p => new GetPermissionByIdResponseModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    ForType = p.ForType,
                    UserId = p.UserId,
                    ResponsibilityId = p.ResponsibilityId,
                    PermissionScreens = p.PermissionScreens.Select(ps => new PermissionScreenResponseModel
                    {
                        ScreenId = ps.ScreenId,
                        //ScreenCode = ps.ScreenCode,
                        ScreenActions = ps.PermissionScreenActions.Select(psa => psa.ActionCode).ToList()
                    }).ToList(),
                    IsActive = p.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permission;

        }
        public async Task<GetPermissionByIdResponseModel> CheckAndGetPermission(CheckAndGetPermissionModel model)
        {
            #region Validation

            if (model.ResponsibilityId == null && model.UserId == null)
                throw new BusinessValidationException(LeillaKeys.SorryYouMustChooseResponsibilityOrUser);

            if (model.ResponsibilityId != null && model.UserId != null)
                throw new BusinessValidationException(LeillaKeys.SorryYouMustNotChooseResponsibilityAndUserAtTheSameTime);

            #endregion

            var permission = await repositoryManager.PermissionRepository
                .Get(permission => permission.CompanyId == requestInfo.CompanyId && (model.ResponsibilityId != null && model.ResponsibilityId == permission.ResponsibilityId ||
                model.UserId != null && model.UserId == permission.UserId) && !permission.IsDeleted)
                .Select(p => new GetPermissionByIdResponseModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    ForType = p.ForType,
                    UserId = p.UserId,
                    ResponsibilityId = p.ResponsibilityId,
                    PermissionScreens = p.PermissionScreens.Select(ps => new PermissionScreenResponseModel
                    {
                        ScreenId = ps.ScreenId,
                        //ScreenCode = ps.ScreenCode,
                        ScreenActions = ps.PermissionScreenActions.Select(psa => psa.ActionCode).ToList()
                    }).ToList(),
                    IsActive = p.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permission;

        }
        public async Task<bool> Enable(int permissionId)
        {
            var responsibility = await repositoryManager.PermissionRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive
                && ((requestInfo.CompanyId > 0 && d.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && d.CompanyId == null))
                && d.Type == requestInfo.AuthenticationType && d.Id == permissionId) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);
            responsibility.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var responsibility = await repositoryManager.PermissionRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted
                && ((requestInfo.CompanyId > 0 && d.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && d.CompanyId == null))
                && d.Type == requestInfo.AuthenticationType && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);
            responsibility.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int permissiond)
        {
            var permission = await repositoryManager.PermissionRepository.
                GetEntityByConditionWithTrackingAsync(permission => !permission.IsDeleted && permission.Id == permissiond &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.Type == requestInfo.AuthenticationType) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);
            permission.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetPermissionsInformationsResponseDTO> GetPermissionsInformations()
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var query = permissionRepository.Get(permission =>
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null))
                && permission.Type == requestInfo.AuthenticationType);

            #region Handle Response

            return new GetPermissionsInformationsResponseDTO
            {
                TotalCount = await query.Where(permission => !permission.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(permission => !permission.IsDeleted && permission.IsActive).CountAsync(),
                NotActiveCount = await query.Where(permission => !permission.IsDeleted && !permission.IsActive).CountAsync(),
                DeletedCount = await query.Where(permission => permission.IsDeleted).CountAsync()
            };

            #endregion
        }
        public async Task<bool> CheckScreenInPlan(CheckScreenInPlanModel model)
        {
            var currentCompanyId = model.AuthenticationType == AuthenticationType.AdminPanel ? null : model.CompanyId;

            var currentType = model.AuthenticationType == AuthenticationType.AdminPanel ?
                AuthenticationType.AdminPanel : model.AuthenticationType == AuthenticationType.DawemAdmin &&
                model.ApplicationType == ApplicationType.Web ? AuthenticationType.DawemAdmin :
                AuthenticationType.DawemEmployee;

            var checkScreenInPlan = false;

            #region Handle Plan Screens

            var getScreenId = await repositoryManager.MenuItemRepository.
                    Get(s => !s.IsDeleted && s.IsActive && s.MenuItemCode == model.ScreenCode &&
                    s.AuthenticationType == currentType).
                    Select(s => s.Id).
                    FirstOrDefaultAsync();

            var allScreensAvailableForPlan = true;
            var planScreensIds = new List<int>();

            if (currentCompanyId > 0)
            {
                var getCompanySubscriptionPlanId = await repositoryManager.CompanyRepository.
                    Get(c => !c.IsDeleted && c.Id == currentCompanyId).
                    Select(c => c.Subscription.PlanId).
                    FirstOrDefaultAsync();

                if (getCompanySubscriptionPlanId > 0)
                {
                    var getPlanInfo = await repositoryManager.PlanRepository.
                                    Get(c => !c.IsDeleted && c.Id == getCompanySubscriptionPlanId).
                                    Select(c => new
                                    {
                                        c.AllScreensAvailable,
                                        PlanScreens = c.PlanScreens.Any() ?
                                        c.PlanScreens.Select(ps => ps.ScreenId).ToList() : null
                                    }).FirstOrDefaultAsync();

                    if (getPlanInfo != null)
                    {
                        allScreensAvailableForPlan = getPlanInfo.AllScreensAvailable;

                        if (!getPlanInfo.AllScreensAvailable)
                        {
                            planScreensIds = getPlanInfo.PlanScreens;
                        }
                    }
                }

            }

            #endregion

            if (allScreensAvailableForPlan || (planScreensIds != null && planScreensIds.Any(screenId => screenId == getScreenId)))
            {
                checkScreenInPlan = true;
            }

            return checkScreenInPlan;
        }
        public async Task<bool> CheckUserPermission(CheckUserPermissionModel model)
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var permissionScreenActionRepository = repositoryManager.PermissionScreenActionRepository;
            var userResponsibilityRepository = repositoryManager.UserResponsibilityRepository;
            var currentUserId = model?.UserId ?? requestInfo.UserId;
            var currentCompanyId = model.AuthenticationType == AuthenticationType.AdminPanel ? null : model.CompanyId;

            var currentType = model.AuthenticationType == AuthenticationType.AdminPanel ?
                AuthenticationType.AdminPanel : model.AuthenticationType == AuthenticationType.DawemAdmin &&
                model.ApplicationType == ApplicationType.Web ? AuthenticationType.DawemAdmin :
                AuthenticationType.DawemEmployee;

            var getScreenId = await repositoryManager.MenuItemRepository.
                    Get(s => !s.IsDeleted && s.IsActive && s.MenuItemCode == model.ScreenCode &&
                    s.AuthenticationType == currentType).
                    Select(s => s.Id).
                    FirstOrDefaultAsync();

            var isUserIsAdmin = await repositoryManager.UserRepository
              .Get(p => !p.IsDeleted && p.IsActive &&
              p.Type == model.AuthenticationType &&
              p.CompanyId == currentCompanyId && p.IsAdmin &&
              p.Id == currentUserId).AnyAsync();

            var checkIfHasPermission = false;

            if (isUserIsAdmin)
            {
                checkIfHasPermission = true;
            }
            else
            {
                var isUserHasPermissions = await repositoryManager.PermissionRepository
                    .Get(p => !p.IsDeleted && p.IsActive &&
                    p.CompanyId == currentCompanyId
                    && p.Type == model.AuthenticationType && p.UserId == currentUserId).AnyAsync();

                var getUserResponsibilitiesIds = await userResponsibilityRepository
                    .Get(u => u.UserId == currentUserId)
                    .Select(ur => ur.ResponsibilityId)
                    .ToListAsync();

                var isUserResponsibilitiesHasPermissions = await repositoryManager.PermissionRepository
                            .Get(p => !p.IsDeleted && p.IsActive &&
                            p.CompanyId == currentCompanyId
                             && p.Type == model.AuthenticationType &&
                            p.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(p.ResponsibilityId.Value)).AnyAsync();

                if (isUserHasPermissions)
                {
                    checkIfHasPermission = checkIfHasPermission || await permissionScreenActionRepository.
                        Get(p => !p.IsDeleted && p.IsActive && !p.PermissionScreen.IsDeleted &&
                        !p.PermissionScreen.Permission.IsDeleted &&
                        p.PermissionScreen.Permission.CompanyId == currentCompanyId &&
                        p.PermissionScreen.Permission.Type == model.AuthenticationType &&
                        p.PermissionScreen.ScreenId == getScreenId &&
                        p.ActionCode == model.ActionCode && p.PermissionScreen.Permission.UserId == model.UserId).
                        AnyAsync();
                }
                if (isUserResponsibilitiesHasPermissions)
                {
                    checkIfHasPermission = checkIfHasPermission || await permissionScreenActionRepository
                                 .Get(p => !p.IsDeleted && p.IsActive && !p.PermissionScreen.IsDeleted &&
                                 !p.PermissionScreen.Permission.IsDeleted &&
                                 p.PermissionScreen.Permission.CompanyId == currentCompanyId &&
                                 p.PermissionScreen.Permission.Type == model.AuthenticationType &&
                                 p.PermissionScreen.ScreenId == getScreenId
                                 && p.ActionCode == model.ActionCode && p.PermissionScreen.Permission.ResponsibilityId > 0
                                 && getUserResponsibilitiesIds.Contains(p.PermissionScreen.Permission.ResponsibilityId.Value)).AnyAsync();
                }
            }

            return checkIfHasPermission;
        }
        public async Task<GetUserPermissionsResponseModel> OldGetCurrentUserPermissions(GetCurrentUserMenuItemsModel model = null)
        {
            var resonse = new GetUserPermissionsResponseModel();
            var currentUserId = model?.UserId ?? requestInfo.UserId;
            var currentCompanyId = model?.CompanyId ?? (requestInfo.CompanyId > 0 ? requestInfo.CompanyId : null);

            var authenticationType = model.AuthenticationType == AuthenticationType.AdminPanel ?
                AuthenticationType.AdminPanel : model.AuthenticationType == AuthenticationType.DawemAdmin &&
                requestInfo.ApplicationType == ApplicationType.Web ? AuthenticationType.DawemAdmin :
                AuthenticationType.DawemEmployee;

            var lang = requestInfo.Lang;
            var permissionScreenRepository = repositoryManager.PermissionScreenRepository;
            var userResponsibilityRepository = repositoryManager.UserResponsibilityRepository;
            var screenNameSuffix = authenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

            var isUserIsAdmin = await repositoryManager.UserRepository
               .Get(p => !p.IsDeleted && p.IsActive &&
               p.CompanyId == currentCompanyId
                && p.Type == authenticationType && p.IsAdmin &&
               p.Id == currentUserId).AnyAsync();

            if (isUserIsAdmin)
            {
                resonse.IsAdmin = true;
                resonse.UserPermissions = null;
            }
            else
            {
                var isUserHasPermission = await repositoryManager.PermissionRepository
                    .Get(p => !p.IsDeleted && p.IsActive &&
                    p.CompanyId == currentCompanyId
                    && p.Type == authenticationType && p.UserId == currentUserId).AnyAsync();

                var getUserResponsibilitiesIds = await userResponsibilityRepository
                    .Get(u => u.UserId == currentUserId)
                    .Select(ur => ur.ResponsibilityId)
                    .ToListAsync();
                var isUserResponsibilitiesHasPermission = await repositoryManager.PermissionRepository
                            .Get(p => !p.IsDeleted && p.IsActive &&
                            p.CompanyId == currentCompanyId
                             && p.Type == authenticationType &&
                            p.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(p.ResponsibilityId.Value)).AnyAsync();

                if (isUserHasPermission)
                {
                    var getUserPermissions = await permissionScreenRepository.Get(ps => !ps.IsDeleted && !ps.Permission.IsDeleted
                    && ps.Permission.CompanyId == currentCompanyId
                    && ps.Permission.Type == authenticationType &&
                    ps.Permission.UserId == currentUserId)
                        .GroupBy(ps => ps.ScreenId)
                        .Select(g => new PermissionScreenResponseWithNamesModel
                        {
                            ScreenName = g.First().Screen.MenuItemNameTranslations.FirstOrDefault(s => s.Language.ISO2 == requestInfo.Lang).Name,
                            PermissionScreenActions = g.SelectMany(a => a.PermissionScreenActions)
                            .GroupBy(a => a.ActionCode).Select(g => new PermissionScreenActionResponseWithNamesModel
                            {
                                ActionCode = g.First().ActionCode,
                                ActionName = TranslationHelper.GetTranslation(g.First().ActionCode.ToString(), lang)
                            }).OrderBy(a => a.ActionCode).ToList()
                        }).OrderBy(ps => ps.ScreenName).ToListAsync();

                    resonse.UserPermissions.AddRange(getUserPermissions);
                }
                if (isUserResponsibilitiesHasPermission)
                {
                    var getResponsibilitiesPermissions = await permissionScreenRepository.
                        Get(ps => !ps.IsDeleted && !ps.Permission.IsDeleted &&
                        ps.Permission.CompanyId == currentCompanyId &&
                        ps.Permission.Type == authenticationType &&
                        ps.Permission.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(ps.Permission.ResponsibilityId.Value)).
                        GroupBy(ps => ps.ScreenId).
                        Select(g => new PermissionScreenResponseWithNamesModel
                        {
                            ScreenName = g.First().Screen.MenuItemNameTranslations.FirstOrDefault(s => s.Language.ISO2 == requestInfo.Lang).Name,
                            PermissionScreenActions = g.SelectMany(a => a.PermissionScreenActions)
                        .GroupBy(a => a.ActionCode).Select(g => new PermissionScreenActionResponseWithNamesModel
                        {
                            ActionCode = g.First().ActionCode,
                            ActionName = TranslationHelper.GetTranslation(g.First().ActionCode.ToString(), lang)
                        }).OrderBy(a => a.ActionCode).ToList()
                        }).OrderBy(ps => ps.ScreenName).ToListAsync();

                    resonse.UserPermissions.AddRange(getResponsibilitiesPermissions);
                }
            }

            return resonse;
        }
        public async Task<GetUserMenuItemsWithAvailableActionsResponse> GetCurrentUserMenuItems(GetCurrentUserMenuItemsModel model = null)
        {
            var resonse = new GetUserMenuItemsWithAvailableActionsResponse();
            var currentUserId = model?.UserId ?? requestInfo.UserId;
            var currentCompanyId = model?.CompanyId ?? (requestInfo.CompanyId > 0 ? requestInfo.CompanyId : null);
            var currentAuthenticationType = model?.AuthenticationType != null ? model?.AuthenticationType : 
                requestInfo.AuthenticationType;

            var authenticationType = currentAuthenticationType == AuthenticationType.AdminPanel ?
                AuthenticationType.AdminPanel : currentAuthenticationType == AuthenticationType.DawemAdmin &&
                requestInfo.ApplicationType == ApplicationType.Web ? AuthenticationType.DawemAdmin :
                AuthenticationType.DawemEmployee;

            var lang = requestInfo.Lang;
            var permissionScreenRepository = repositoryManager.PermissionScreenRepository;
            var userResponsibilityRepository = repositoryManager.UserResponsibilityRepository;
            var screenNameSuffix = authenticationType == AuthenticationType.AdminPanel ? LeillaKeys.AdminPanelScreen :
                    LeillaKeys.DawemScreen;

            var isUserIsAdmin = await repositoryManager.UserRepository
               .Get(p => !p.IsDeleted && p.IsActive &&
               p.CompanyId == currentCompanyId
                && p.Type == authenticationType && p.IsAdmin &&
               p.Id == currentUserId).AnyAsync();

            var criteria = new GetScreensCriteria
            {
                IsActive = true,
                AuthenticationType = authenticationType,
                CompanyId = currentCompanyId
            };

            if (isUserIsAdmin)
            {
                resonse.MenuItems = await GetMenuItems(criteria);
            }
            else
            {
                var isUserHasPermission = await repositoryManager.PermissionRepository
                    .Get(p => !p.IsDeleted && p.IsActive &&
                    p.CompanyId == currentCompanyId
                    && p.Type == authenticationType && p.UserId == currentUserId).AnyAsync();

                var getUserResponsibilitiesIds = await userResponsibilityRepository
                    .Get(u => u.UserId == currentUserId)
                    .Select(ur => ur.ResponsibilityId)
                    .ToListAsync();
                var isUserResponsibilitiesHasPermission = await repositoryManager.PermissionRepository
                            .Get(p => !p.IsDeleted && p.IsActive &&
                            p.CompanyId == currentCompanyId
                             && p.Type == authenticationType &&
                            p.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(p.ResponsibilityId.Value)).AnyAsync();

                var screensIds = new List<int>();

                if (isUserHasPermission)
                {
                    var getUserPermissionsScreensIds = await permissionScreenRepository.Get(ps => !ps.IsDeleted && !ps.Permission.IsDeleted
                    && ps.Permission.CompanyId == currentCompanyId
                    && ps.Permission.Type == authenticationType &&
                    ps.Permission.UserId == currentUserId).
                    Select(g => g.ScreenId).
                    ToListAsync();

                    screensIds.AddRange(getUserPermissionsScreensIds);
                }
                if (isUserResponsibilitiesHasPermission)
                {
                    var getResponsibilitiesPermissionsScreensIds = await permissionScreenRepository.
                        Get(ps => !ps.IsDeleted && !ps.Permission.IsDeleted &&
                        ps.Permission.CompanyId == currentCompanyId &&
                        ps.Permission.Type == authenticationType &&
                        ps.Permission.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(ps.Permission.ResponsibilityId.Value)).
                        Select(g => g.ScreenId).
                        ToListAsync();

                    screensIds.AddRange(getResponsibilitiesPermissionsScreensIds);
                }

                criteria.ScreensIds = screensIds;
                resonse.MenuItems = await GetMenuItems(criteria);
            }

            return resonse;
        }
        private async Task<List<MenuItemWithAvailableActionsDTO>> GetMenuItems(GetScreensCriteria criteria)
        {
            var allScreensResponse = await screenBLC.GetAllScreensWithAvailableActions(criteria);
            var allScreens = allScreensResponse.MenuItemsTypes.SelectMany(m => m.MenuItems).ToList();

            return allScreens;
        }
    }
}

