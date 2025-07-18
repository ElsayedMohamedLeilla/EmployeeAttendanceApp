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
using Dawem.Models.Dtos.Dawem.Core.JustificationsTypes;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.Dawem.Core.JustificationsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Dawem.Core.JustificationsTypes
{
    public class JustificationTypeBL : IJustificationTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IJustificationTypeBLValidation justificationTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public JustificationTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IJustificationTypeBLValidation _justificationsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            justificationTypeBLValidation = _justificationsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateJustificationsTypeDTO model)
        {
            #region Business Validation

            await justificationTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert JustificationsType

            #region Set JustificationsType code

            var getNextCode = await repositoryManager.JustificationsTypeRepository
                .Get(e => e.CompanyId == requestInfo.CompanyId)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var justificationsType = mapper.Map<JustificationType>(model);
            justificationsType.CompanyId = requestInfo.CompanyId;
            justificationsType.AddUserId = requestInfo.UserId;
            justificationsType.Code = getNextCode;
            repositoryManager.JustificationsTypeRepository.Insert(justificationsType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return justificationsType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateJustificationsTypeDTO model)
        {
            #region Business Validation

            await justificationTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();
            #region Update JustificationsType
            var getJustificationsType = await repositoryManager.JustificationsTypeRepository.GetByIdAsync(model.Id);
            getJustificationsType.Name = model.Name;
            getJustificationsType.IsActive = model.IsActive;
            getJustificationsType.ModifiedDate = DateTime.Now;
            getJustificationsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetJustificationsTypeResponseDTO> Get(GetJustificationsTypesCriteria criteria)
        {
            var justificationsTypeRepository = repositoryManager.JustificationsTypeRepository;
            var query = justificationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = justificationsTypeRepository.OrderBy(query, nameof(JustificationType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var justificationsTypesList = await queryPaged.Select(e => new GetJustificationsTypeResponseModelDTO
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                IsActive = e.IsActive,
            }).ToListAsync();

            return new GetJustificationsTypeResponseDTO
            {
                JustificationsTypes = justificationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetJustificationsTypeDropDownResponseDTO> GetForDropDown(GetJustificationsTypesCriteria criteria)
        {
            criteria.IsActive = true;
            var JustificationsTypeRepository = repositoryManager.JustificationsTypeRepository;
            var query = JustificationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = JustificationsTypeRepository.OrderBy(query, nameof(JustificationType.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var justificationsTypesList = await queryPaged.Select(e => new GetJustificationsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetJustificationsTypeDropDownResponseDTO
            {
                JustificationsTypes = justificationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetJustificationsTypeInfoResponseDTO> GetInfo(int justificationsTypeId)
        {
            var justificationsType = await repositoryManager.JustificationsTypeRepository.Get(e => e.Id == justificationsTypeId && !e.IsDeleted)
                .Select(e => new GetJustificationsTypeInfoResponseDTO
                {
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);

            return justificationsType;
        }
        public async Task<GetJustificationsTypeByIdResponseDTO> GetById(int justificationsTypeId)
        {
            var justificationsType = await repositoryManager.JustificationsTypeRepository.Get(e => e.Id == justificationsTypeId && !e.IsDeleted)
                .Select(e => new GetJustificationsTypeByIdResponseDTO
                {
                    Id = e.Id,
                    Code = e.Code,
                    Name = e.Name,
                    IsActive = e.IsActive,

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);

            return justificationsType;

        }
        public async Task<bool> Delete(int JustificationsTypeId)
        {
            var justificationsType = await repositoryManager.JustificationsTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == JustificationsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);
            justificationsType.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetJustificationsTypesInformationsResponseDTO> GetJustificationTypesInformations()
        {
            var justificationRepository = repositoryManager.JustificationsTypeRepository;
            var query = justificationRepository.Get(justification => justification.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetJustificationsTypesInformationsResponseDTO
            {
                TotalCount = await query.Where(justification => !justification.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(justification => !justification.IsDeleted && justification.IsActive).CountAsync(),
                NotActiveCount = await query.Where(justification => !justification.IsDeleted && !justification.IsActive).CountAsync(),
                DeletedCount = await query.Where(justification => justification.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}
