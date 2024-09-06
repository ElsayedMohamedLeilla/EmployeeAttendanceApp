using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultPermissionsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultPermissionTypeBL : IDefaultPermissionTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultPermissionTypeBLValidation PermissionTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultPermissionTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultPermissionTypeBLValidation _PermissionsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            PermissionTypeBLValidation = _PermissionsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultPermissionsTypeDTO model)
        {
            #region Business Validation

            await PermissionTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert PermissionsType

            #region Set PermissionsType code

            var getNextCode = await repositoryManager.DefaultPermissionTypeRepository
                .Get(e => e.LookupType == LookupsType.PermissionsTypes)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var PermissionType = mapper.Map<DefaultLookup>(model);
            PermissionType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            PermissionType.LookupType = LookupsType.PermissionsTypes;
            PermissionType.AddUserId = requestInfo.UserId;
            PermissionType.Code = getNextCode;
            repositoryManager.DefaultPermissionTypeRepository.Insert(PermissionType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return PermissionType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultPermissionsTypeDTO model)
        {
            #region Business Validation

            await PermissionTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update PermissionsType
            var getPermissionsType = await repositoryManager.DefaultPermissionTypeRepository.GetByIdAsync(model.Id);
            getPermissionsType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getPermissionsType.DefaultType = model.DefaultType;
            getPermissionsType.IsActive = model.IsActive;
            getPermissionsType.ModifiedDate = DateTime.Now;
            getPermissionsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getPermissionsType.Id)
                    .ToListAsync();

            var existingNameTranslationsIds = exisNameTranslationsDbList.Select(e => e.Id)
                .ToList();

            var addedPlanNameTranslations = model.NameTranslations != null ? model.NameTranslations
                .Where(ge => !existingNameTranslationsIds.Contains(ge.Id))
                .Select(ge => new DefaultLookupsNameTranslation
                {
                    DefaultLookupId = model.Id,
                    LanguageId = ge.LanguageId,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<DefaultLookupsNameTranslation>();

            var removedPlanNameTranslationsIds = exisNameTranslationsDbList
                .Where(ge => model.NameTranslations == null || !model.NameTranslations.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedPlanNameTranslations = exisNameTranslationsDbList
                .Where(e => removedPlanNameTranslationsIds.Contains(e.Id))
                .ToList();

            var updatedPlanNameTranslations = exisNameTranslationsDbList.
                Where(nt => model.NameTranslations != null && model.NameTranslations.
                Any(mi => mi.Id == nt.Id && (mi.Name != nt.Name || mi.LanguageId != nt.LanguageId))).
                ToList();

            if (removedPlanNameTranslations.Count() > 0)
                repositoryManager.DefaultLookupsNameTranslationRepository.BulkDeleteIfExist(removedPlanNameTranslations);
            if (addedPlanNameTranslations.Count() > 0)
                repositoryManager.DefaultLookupsNameTranslationRepository.BulkInsert(addedPlanNameTranslations);
            if (updatedPlanNameTranslations.Count() > 0)
            {
                var modelNameTranslationsDict = model.NameTranslations?.ToDictionary(mi => mi.Id, mi => mi);

                updatedPlanNameTranslations.ForEach(i =>
                {
                    if (modelNameTranslationsDict != null && modelNameTranslationsDict.TryGetValue(i.Id, out var translation))
                    {
                        i.Name = translation.Name;
                        i.LanguageId = translation.LanguageId;
                    }
                });

                repositoryManager.DefaultLookupsNameTranslationRepository.BulkUpdate(updatedPlanNameTranslations);
            }
            await unitOfWork.SaveAsync();


            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }


        public async Task<GetDefaultPermissionsTypeResponseDTO> Get(GetDefaultPermissionTypeCriteria criteria)
        {
            var PermissionsTypeRepository = repositoryManager.DefaultPermissionTypeRepository;
            var query = PermissionsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = PermissionsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var PermissionsTypesList = await queryPaged.Select(PermissionType => new GetDefaultPermissionsTypeResponseModelDTO
            {
                Id = PermissionType.Id,
                Code = PermissionType.Code,
                Name = PermissionType.Name,
                //DefaultType = PermissionType.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(PermissionType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = PermissionType.IsActive,
            }).ToListAsync();

            return new GetDefaultPermissionsTypeResponseDTO
            {
                DefaultPermissionsTypes = PermissionsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultPermissionsTypeDropDownResponseDTO> GetForDropDown(GetDefaultPermissionTypeCriteria criteria)
        {
            criteria.IsActive = true;
            var PermissionsTypeRepository = repositoryManager.DefaultPermissionTypeRepository;
            var query = PermissionsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = PermissionsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var PermissionsTypesList = await queryPaged.Select(e => new GetDefaultPermissionsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultPermissionsTypeDropDownResponseDTO
            {
                DefaultPermissionsTypes = PermissionsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultPermissionsTypeInfoResponseDTO> GetInfo(int PermissionsTypeId)
        {
            var PermissionsType = await repositoryManager.DefaultPermissionTypeRepository.Get(e => e.Id == PermissionsTypeId && !e.IsDeleted)
                .Select(PermissionType => new GetDefaultPermissionsTypeInfoResponseDTO
                {
                    Code = PermissionType.Code,
                    Name = PermissionType.Name,
                    //DefaultType = PermissionType.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(PermissionType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = PermissionType.IsActive,
                    NameTranslations = PermissionType.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);

            return PermissionsType;
        }
        public async Task<GetDefaultPermissionsTypeByIdResponseDTO> GetById(int PermissionsTypeId)
        {
            var PermissionsType = await repositoryManager.DefaultPermissionTypeRepository.Get(e => e.Id == PermissionsTypeId && !e.IsDeleted)
                .Select(PermissionType => new GetDefaultPermissionsTypeByIdResponseDTO
                {
                    Id = PermissionType.Id,
                    Code = PermissionType.Code,
                    Name = PermissionType.Name,
                    //DefaultType = PermissionType.DefaultType,
                    IsActive = PermissionType.IsActive,
                    NameTranslations = PermissionType.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);

            return PermissionsType;

        }
        public async Task<bool> Delete(int PermissionsTypeId)
        {
            var PermissionsType = await repositoryManager.DefaultPermissionTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == PermissionsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);
            PermissionsType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int PermissionTypeId)
        {
            var PermissionType = await repositoryManager.DefaultPermissionTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == PermissionTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);
            PermissionType.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultPermissionTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryPermissionsTypeNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
