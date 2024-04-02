using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Permissions;
using Dawem.Contract.BusinessValidation.Dawem.Permissions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Permissions.Permissions;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
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
        public PermissionBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IPermissionBLValidation _permissionBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            permissionBLValidation = _permissionBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreatePermissionModel model)
        {
            #region Business Validation

            await permissionBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Permission

            #region Set Permission Code
            var getNextCode = await repositoryManager.PermissionRepository
                .Get(permission => permission.IsForAdminPanel == requestInfo.IsAdminPanel &&
                ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)))
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var permission = mapper.Map<Permission>(model);
            permission.CompanyId = requestInfo.CompanyId;
            permission.AddUserId = requestInfo.UserId;
            permission.IsForAdminPanel = requestInfo.IsAdminPanel;
            permission.Code = getNextCode;
            repositoryManager.PermissionRepository.Insert(permission);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return permission.Id;

            #endregion

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
                && permission.IsForAdminPanel == requestInfo.IsAdminPanel &&
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
                var modelPermissionScreens = model.PermissionScreens;

                var getAddedPermissionScreens = modelPermissionScreens
                    .Where(m => !dbPermissionScreens.Any(d => d.ScreenCode == m.ScreenCode))
                    .Select(m => new PermissionScreen
                    {
                        PermissionId = model.Id,
                        ScreenCode = m.ScreenCode,
                        PermissionScreenActions = m.PermissionScreenActions.Select(m => new PermissionScreenAction
                        {
                            ActionCode = m.ActionCode
                        }).ToList()
                    }).ToList();

                var getDeletedPermissionScreens = dbPermissionScreens
                    .Where(d => !modelPermissionScreens.Any(m => m.ScreenCode == d.ScreenCode))
                    .ToList();

                repositoryManager.PermissionScreenRepository.BulkInsert(getAddedPermissionScreens);
                repositoryManager.PermissionScreenRepository.BulkDeleteIfExist(getDeletedPermissionScreens);

                var getUpdatedPermissionScreens = modelPermissionScreens
                    .Where(d => !getAddedPermissionScreens.Any(d => d.ScreenCode == d.ScreenCode)
                    && !getDeletedPermissionScreens.Any(d => d.ScreenCode == d.ScreenCode))
                    .ToList();


                #region Update Related Screen Actions

                if (getUpdatedPermissionScreens != null && getUpdatedPermissionScreens.Count > 0)
                {
                    foreach (var permissionScreen in getUpdatedPermissionScreens)
                    {
                        var dbPermissionScreen = dbPermissionScreens
                            .FirstOrDefault(ps => ps.ScreenCode == permissionScreen.ScreenCode);
                        var dbPermissionScreenActions = dbPermissionScreen.PermissionScreenActions;
                        var modelPermissionScreenActions = permissionScreen.PermissionScreenActions;

                        var getAddedPermissionScreenActions = modelPermissionScreenActions
                            .Where(m => !dbPermissionScreenActions.Any(d => d.ActionCode == m.ActionCode))
                            .Select(m => new PermissionScreenAction
                            {
                                PermissionScreenId = dbPermissionScreen.Id,
                                ActionCode = m.ActionCode,
                            }).ToList();

                        var getDeletedPermissionScreenActions = dbPermissionScreenActions
                            .Where(d => !modelPermissionScreenActions.Any(m => m.ActionCode == d.ActionCode))
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
            var permission = await repositoryManager.PermissionRepository.Get(permission => permission.Id == permissionId
            && !permission.IsDeleted && ((requestInfo.CompanyId > 0 && permission.CompanyId == requestInfo.CompanyId) ||
                (requestInfo.CompanyId <= 0 && permission.CompanyId == null)) &&
                permission.IsForAdminPanel == requestInfo.IsAdminPanel)
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
                        ScreenCode = ps.ScreenCode,
                        ScreenName = TranslationHelper.GetTranslation(ps.ScreenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang),
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

            var permissionScreensList = await queryPaged.Select(ps => new GetPermissionScreenInfoModel
            {
                ScreenCode = ps.ScreenCode,
                ScreenName = TranslationHelper.GetTranslation(ps.ScreenCode.ToString() + LeillaKeys.Screen, requestInfo.Lang),
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
                permission.IsForAdminPanel == requestInfo.IsAdminPanel)
                .Select(p => new GetPermissionByIdResponseModel
                {
                    Id = p.Id,
                    Code = p.Code,
                    ForType = p.ForType,
                    UserId = p.UserId,
                    ResponsibilityId = p.ResponsibilityId,
                    PermissionScreens = p.PermissionScreens.Select(ps => new PermissionScreenResponseModel
                    {
                        ScreenCode = ps.ScreenCode,
                        PermissionScreenActions = ps.PermissionScreenActions.Select(psa => new PermissionScreenActionResponseModel
                        {
                            ActionCode = psa.ActionCode
                        }).ToList()
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
                        ScreenCode = ps.ScreenCode,
                        PermissionScreenActions = ps.PermissionScreenActions.Select(psa => new PermissionScreenActionResponseModel
                        {
                            ActionCode = psa.ActionCode
                        }).ToList()
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
                && d.IsForAdminPanel == requestInfo.IsAdminPanel && d.Id == permissionId) ??
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
                && d.IsForAdminPanel == requestInfo.IsAdminPanel && d.IsActive && d.Id == model.Id) ??
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
                permission.IsForAdminPanel == requestInfo.IsAdminPanel) ??
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
                && permission.IsForAdminPanel == requestInfo.IsAdminPanel);

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
        public async Task<bool> CheckUserPermission(CheckUserPermissionModel model)
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var permissionScreenActionRepository = repositoryManager.PermissionScreenActionRepository;
            var userResponsibilityRepository = repositoryManager.UserResponsibilityRepository;
            var currentUserId = model?.UserId ?? requestInfo.UserId;
            var currentCompanyId = model.IsForAdminPanel ? null : model.CompanyId;

            var isUserHasPermission = await repositoryManager.PermissionRepository
                .Get(p => !p.IsDeleted && p.IsActive &&
                p.IsForAdminPanel == model.IsForAdminPanel &&
                p.CompanyId == currentCompanyId && p.UserId == model.UserId).AnyAsync();

            var isUserIsAdmin = await repositoryManager.UserRepository
              .Get(p => !p.IsDeleted && p.IsActive &&
              p.IsForAdminPanel == model.IsForAdminPanel &&
              p.CompanyId == currentCompanyId && p.IsAdmin &&
              p.Id == currentUserId).AnyAsync();

            if (isUserIsAdmin)
            {
                return true;
            }
            else if (isUserHasPermission)
            {
                var checkIfHasPermission = await permissionScreenActionRepository
                    .Get(p => !p.IsDeleted && p.IsActive && !p.PermissionScreen.IsDeleted && !p.PermissionScreen.Permission.IsDeleted &&
                    p.PermissionScreen.Permission.CompanyId == currentCompanyId &&
                    p.PermissionScreen.Permission.IsForAdminPanel == model.IsForAdminPanel &&
                    p.PermissionScreen.ScreenCode == model.ScreenCode
                     && p.ActionCode == model.ActionCode && p.PermissionScreen.Permission.UserId == model.UserId)
                    .AnyAsync();

                if (!checkIfHasPermission)
                {
                    return false;
                }
            }
            else
            {
                var getUserResponsibilitiesIds = await userResponsibilityRepository
                    .Get(u => !u.IsDeleted && u.UserId == model.UserId && u.User.IsForAdminPanel == model.IsForAdminPanel)
                    .Select(ur => ur.ResponsibilityId)
                    .ToListAsync();

                if (getUserResponsibilitiesIds != null && getUserResponsibilitiesIds.Count > 0)
                {
                    var isResponsibilitiesHasPermission = await repositoryManager.PermissionRepository
                    .Get(p => !p.IsDeleted && p.IsActive &&
                    p.CompanyId == currentCompanyId && p.ResponsibilityId > 0 && 
                    p.User.IsForAdminPanel == model.IsForAdminPanel && 
                    getUserResponsibilitiesIds.Contains(p.ResponsibilityId.Value)).AnyAsync();

                    if (isResponsibilitiesHasPermission)
                    {
                        var checkIfHasPermission = await permissionScreenActionRepository
                            .Get(p => !p.IsDeleted && p.IsActive && !p.PermissionScreen.IsDeleted && 
                            !p.PermissionScreen.Permission.IsDeleted &&
                            p.PermissionScreen.Permission.CompanyId == currentCompanyId &&
                            p.PermissionScreen.Permission.IsForAdminPanel == model.IsForAdminPanel &&
                            p.PermissionScreen.ScreenCode == model.ScreenCode
                            && p.ActionCode == model.ActionCode && p.PermissionScreen.Permission.ResponsibilityId > 0
                            && getUserResponsibilitiesIds.Contains(p.PermissionScreen.Permission.ResponsibilityId.Value)).AnyAsync();

                        if (!checkIfHasPermission)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        public async Task<GetUserPermissionsResponseModel> GetCurrentUserPermissions(GetCurrentUserPermissionsModel model = null)
        {
            var resonse = new GetUserPermissionsResponseModel();
            var currentUserId = model?.UserId ?? requestInfo.UserId;
            var currentCompanyId = model?.CompanyId;
            var lang = requestInfo.Lang;
            var permissionScreenRepository = repositoryManager.PermissionScreenRepository;

            var isUserIsAdmin = await repositoryManager.UserRepository
               .Get(p => !p.IsDeleted && p.IsActive &&
               p.CompanyId == currentCompanyId
                && p.IsForAdminPanel == model.IsForAdminPanel && p.IsAdmin &&
               p.Id == currentUserId).AnyAsync();

            var isUserHasPermission = await repositoryManager.PermissionRepository
                .Get(p => !p.IsDeleted && p.IsActive &&
                p.CompanyId == currentCompanyId
                && p.IsForAdminPanel == model.IsForAdminPanel && p.UserId == currentUserId).AnyAsync();

            if (isUserIsAdmin)
            {
                resonse.IsAdmin = true;
                resonse.UserPermissions = null;
            }
            else if (isUserHasPermission)
            {
                var getResponsibilitiesPermissions = await permissionScreenRepository.Get(ps => !ps.IsDeleted && !ps.Permission.IsDeleted
                && ps.Permission.CompanyId == currentCompanyId
                && ps.Permission.IsForAdminPanel == model.IsForAdminPanel &&
                ps.Permission.UserId == currentUserId)
                    .GroupBy(ps => ps.ScreenCode)
                    .Select(g => new PermissionScreenResponseWithNamesModel
                    {
                        ScreenCode = g.First().ScreenCode,
                        ScreenName = TranslationHelper.GetTranslation(g.First().ScreenCode.ToString() + LeillaKeys.Screen, lang),
                        PermissionScreenActions = g.SelectMany(a => a.PermissionScreenActions)
                        .GroupBy(a => a.ActionCode).Select(g => new PermissionScreenActionResponseWithNamesModel
                        {
                            ActionCode = g.First().ActionCode,
                            ActionName = TranslationHelper.GetTranslation(g.First().ActionCode.ToString(), lang)
                        }).OrderBy(a => a.ActionCode).ToList()
                    }).OrderBy(ps => ps.ScreenCode).ToListAsync();

                resonse.UserPermissions = getResponsibilitiesPermissions;
            }
            else
            {
                var userResponsibilityRepository = repositoryManager.UserResponsibilityRepository;
                var getUserResponsibilitiesIds = await userResponsibilityRepository
                    .Get(u => u.UserId == currentUserId)
                    .Select(ur => ur.ResponsibilityId)
                    .ToListAsync();

                if (getUserResponsibilitiesIds != null && getUserResponsibilitiesIds.Count > 0)
                {
                    var isResponsibilitiesHasPermission = await repositoryManager.PermissionRepository
                    .Get(p => !p.IsDeleted && p.IsActive &&
                    p.CompanyId == currentCompanyId
                     && p.IsForAdminPanel == model.IsForAdminPanel && 
                    p.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(p.ResponsibilityId.Value)).AnyAsync();

                    if (isResponsibilitiesHasPermission)
                    {
                        var getResponsibilitiesPermissions = await permissionScreenRepository.Get(ps => !ps.IsDeleted && !ps.Permission.IsDeleted
                        && ps.Permission.CompanyId == currentCompanyId
                            && ps.Permission.IsForAdminPanel == model.IsForAdminPanel &&
                        ps.Permission.ResponsibilityId > 0 && getUserResponsibilitiesIds.Contains(ps.Permission.ResponsibilityId.Value))
                        .GroupBy(ps => ps.ScreenCode)
                        .Select(g => new PermissionScreenResponseWithNamesModel
                        {
                            ScreenCode = g.First().ScreenCode,
                            ScreenName = TranslationHelper.GetTranslation(g.First().ScreenCode.ToString() + LeillaKeys.Screen, lang),
                            PermissionScreenActions = g.SelectMany(a => a.PermissionScreenActions)
                            .GroupBy(a => a.ActionCode).Select(g => new PermissionScreenActionResponseWithNamesModel
                            {
                                ActionCode = g.First().ActionCode,
                                ActionName = TranslationHelper.GetTranslation(g.First().ActionCode.ToString(), lang)
                            }).OrderBy(a => a.ActionCode).ToList()
                        }).OrderBy(ps => ps.ScreenCode).ToListAsync();

                        resonse.UserPermissions = getResponsibilitiesPermissions;
                    }
                }
            }

            return resonse;
        }
    }
}

