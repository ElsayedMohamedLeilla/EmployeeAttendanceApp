﻿using AutoMapper;
using Dawem.Contract.BusinessLogic.Dawem.Core;
using Dawem.Contract.BusinessValidation.Dawem.Core;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Dtos.Dawem.Core.PermissionsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.PermissionsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.PermissionsTypes
{
    public class PermissionTypeBL : IPermissionTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IPermissionsTypeBLValidation permissionTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public PermissionTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IPermissionsTypeBLValidation _PermissionsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            permissionTypeBLValidation = _PermissionsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreatePermissionTypeDTO model)
        {
            #region Business Validation

            await permissionTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert PermissionsType

            #region Set PermissionsType code

            var getNextCode = await repositoryManager.PermissionsTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var PermissionsType = mapper.Map<PermissionType>(model);
            PermissionsType.CompanyId = requestInfo.CompanyId;
            PermissionsType.AddUserId = requestInfo.UserId;
            PermissionsType.Code = getNextCode;
            repositoryManager.PermissionsTypeRepository.Insert(PermissionsType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return PermissionsType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdatePermissionTypeDTO model)
        {
            #region Business Validation

            await permissionTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();
            #region Update PermissionsType
            var getPermissionsType = await repositoryManager.PermissionsTypeRepository.GetByIdAsync(model.Id);
            getPermissionsType.Name = model.Name;
            getPermissionsType.IsActive = model.IsActive;
            getPermissionsType.ModifiedDate = DateTime.Now;
            getPermissionsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetPermissionsTypeResponseDTO> Get(GetPermissionsTypesCriteria criteria)
        {
            var PermissionsTypeRepository = repositoryManager.PermissionsTypeRepository;
            var query = PermissionsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = PermissionsTypeRepository.OrderBy(query, nameof(PermissionType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var PermissionsTypesList = await queryPaged.Select(e => new GetPermissionsTypeResponseModelDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();

            return new GetPermissionsTypeResponseDTO
            {
                PermissionsTypes = PermissionsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetPermissionsTypeDropDownResponseDTO> GetForDropDown(GetPermissionsTypesCriteria criteria)
        {
            criteria.IsActive = true;
            var PermissionsTypeRepository = repositoryManager.PermissionsTypeRepository;
            var query = PermissionsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = PermissionsTypeRepository.OrderBy(query, nameof(PermissionType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var PermissionsTypesList = await queryPaged.Select(e => new GetPermissionsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetPermissionsTypeDropDownResponseDTO
            {
                PermissionsTypes = PermissionsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetPermissionsTypeInfoResponseDTO> GetInfo(int PermissionsTypeId)
        {
            var PermissionsType = await repositoryManager.PermissionsTypeRepository.Get(e => e.Id == PermissionsTypeId && !e.IsDeleted)
                .Select(e => new GetPermissionsTypeInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);

            return PermissionsType;
        }
        public async Task<GetPermissionsTypeByIdResponseDTO> GetById(int PermissionsTypeId)
        {
            var PermissionsType = await repositoryManager.PermissionsTypeRepository.Get(e => e.Id == PermissionsTypeId && !e.IsDeleted)
                .Select(e => new GetPermissionsTypeByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);

            return PermissionsType;

        }
        public async Task<bool> Delete(int PermissionsTypeId)
        {
            var permissionsType = await repositoryManager.PermissionsTypeRepository
                .GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == PermissionsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);

            permissionsType.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetPermissionsTypesInformationsResponseDTO> GetPermissionTypesInformations()
        {
            var permissionRepository = repositoryManager.PermissionsTypeRepository;
            var query = permissionRepository.Get(permission => permission.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetPermissionsTypesInformationsResponseDTO
            {
                TotalCount = await query.Where(permission => !permission.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(permission => !permission.IsDeleted && permission.IsActive).CountAsync(),
                NotActiveCount = await query.Where(permission => !permission.IsDeleted && !permission.IsActive).CountAsync(),
                DeletedCount = await query.Where(permission => permission.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}
