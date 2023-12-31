using AutoMapper;
using Dawem.Contract.BusinessLogic.Employees;
using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Models.Dtos.Employees.AssignmentTypes;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Permissions.Permissions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Employees
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
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;
            #endregion

            var permission = mapper.Map<Permission>(model);
            permission.CompanyId = requestInfo.CompanyId;
            permission.AddUserId = requestInfo.UserId;

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
                .GetEntityByConditionWithTrackingAsync(permission => !permission.IsDeleted
                && permission.Id == model.Id);


            if (getPermission != null)
            {
                getPermission.Name = model.Name;
                getPermission.IsActive = model.IsActive;
                getPermission.ModifiedDate = DateTime.Now;
                getPermission.ModifyUserId = requestInfo.UserId;
                await unitOfWork.SaveAsync();
                #region Handle Response
                await unitOfWork.CommitAsync();
                return true;
                #endregion
            }
            #endregion

            else
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);


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
            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var permissionsList = await queryPaged.Select(e => new GetPermissionsResponseModel
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();
            return new GetPermissionsResponse
            {
                Permissions = permissionsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetPermissionInfoResponseModel> GetInfo(int PermissionId)
        {
            var permission = await repositoryManager.PermissionRepository.Get(e => e.Id == PermissionId && !e.IsDeleted)
                .Select(e => new GetPermissionInfoResponseModel
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permission;
        }
        public async Task<GetPermissionByIdResponseModel> GetById(int PermissionId)
        {
            var permission = await repositoryManager.PermissionRepository.Get(e => e.Id == PermissionId && !e.IsDeleted)
                .Select(e => new GetPermissionByIdResponseModel
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);

            return permission;

        }
        public async Task<bool> Delete(int permissiond)
        {
            var permission = await repositoryManager.PermissionRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == permissiond) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionNotFound);
            permission.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetPermissionsInformationsResponseDTO> GetPermissionsInformations()
        {
            var permissionRepository = repositoryManager.PermissionRepository;
            var query = permissionRepository.Get(permission => permission.CompanyId == requestInfo.CompanyId);

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
            var userRepository = repositoryManager.UserRepository;

            var getUserRoles = await userRepository.Get(u => !u.IsDeleted && u.Id == model.UserId)
                .Select(u => new { UserRoles = u.UserRoles.Select(ur => ur.RoleId) })
                .FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryUserNotFound);

            var roles = getUserRoles.UserRoles.Select(roleId => roleId).ToList();

            if (roles != null && roles.Count > 0)
            {
                var checkIfHasPermission = await permissionScreenActionRepository
                    .Get(p => !p.IsDeleted && !p.PermissionScreen.IsDeleted && !p.PermissionScreen.Permission.IsDeleted &&
                    p.PermissionScreen.Permission.CompanyId == requestInfo.CompanyId &&
                    p.PermissionScreen.ScreenCode == model.Screen
                     && p.Action == model.Action && roles.Contains(p.PermissionScreen.Permission.RoleId))
                    .AnyAsync();

                if (!checkIfHasPermission)
                {
                    var checkIfHasAnyPermissino = await permissionRepository
                        .Get(p => !p.IsDeleted && roles.Contains(p.RoleId)).AnyAsync();
                    if (checkIfHasAnyPermissino)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

