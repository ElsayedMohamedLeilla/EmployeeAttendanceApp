using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Contract.BusinessValidation.AdminPanel.DefaultLookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Criteria.DefaultLookups;
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultVacationsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultVacationTypeBL : IDefaultVacationTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultVacationTypeBLValidation vacationTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultVacationTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultVacationTypeBLValidation _VacationsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            vacationTypeBLValidation = _VacationsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultVacationsTypeDTO model)
        {
            #region Business Validation

            await vacationTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert VacationsType

            #region Set VacationsType code

            var getNextCode = await repositoryManager.DefaultVacationTypeRepository
                .Get(e => e.LookupType == LookupsType.VacationsTypes)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var vacationType = mapper.Map<DefaultLookup>(model);
            vacationType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            vacationType.LookupType = LookupsType.VacationsTypes;
            vacationType.AddUserId = requestInfo.UserId;
            vacationType.Code = getNextCode;
            repositoryManager.DefaultVacationTypeRepository.Insert(vacationType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return vacationType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultVacationsTypeDTO model)
        {
            #region Business Validation

            await vacationTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update VacationsType
            var getVacationsType = await repositoryManager.DefaultVacationTypeRepository.GetByIdAsync(model.Id);
            getVacationsType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getVacationsType.DefaultType = model.DefaultType;
            getVacationsType.IsActive = model.IsActive;
            getVacationsType.ModifiedDate = DateTime.Now;
            getVacationsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getVacationsType.Id)
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


        public async Task<GetDefaultVacationsTypeResponseDTO> Get(GetDefaultVacationTypeCriteria criteria)
        {
            var VacationsTypeRepository = repositoryManager.DefaultVacationTypeRepository;
            var query = VacationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = VacationsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var VacationsTypesList = await queryPaged.Select(vacationType => new GetDefaultVacationsTypeResponseModelDTO
            {
                Id = vacationType.Id,
                Code = vacationType.Code,
                Name = vacationType.Name,
                //DefaultType = vacationType.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(vacationType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = vacationType.IsActive,
            }).ToListAsync();

            return new GetDefaultVacationsTypeResponseDTO
            {
                DefaultVacationsTypes = VacationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultVacationsTypeDropDownResponseDTO> GetForDropDown(GetDefaultVacationTypeCriteria criteria)
        {
            criteria.IsActive = true;
            var VacationsTypeRepository = repositoryManager.DefaultVacationTypeRepository;
            var query = VacationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = VacationsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var VacationsTypesList = await queryPaged.Select(e => new GetDefaultVacationsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultVacationsTypeDropDownResponseDTO
            {
                DefaultVacationsTypes = VacationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultVacationsTypeInfoResponseDTO> GetInfo(int VacationsTypeId)
        {
            var VacationsType = await repositoryManager.DefaultVacationTypeRepository.Get(e => e.Id == VacationsTypeId && !e.IsDeleted)
                .Select(vacationType => new GetDefaultVacationsTypeInfoResponseDTO
                {
                    Code = vacationType.Code,
                    Name = vacationType.Name,
                    //DefaultType = vacationType.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(vacationType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = vacationType.IsActive,
                    NameTranslations = vacationType.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            return VacationsType;
        }
        public async Task<GetDefaultVacationsTypeByIdResponseDTO> GetById(int VacationsTypeId)
        {
            var VacationsType = await repositoryManager.DefaultVacationTypeRepository.Get(e => e.Id == VacationsTypeId && !e.IsDeleted)
                .Select(vacationType => new GetDefaultVacationsTypeByIdResponseDTO
                {
                    Id = vacationType.Id,
                    Code = vacationType.Code,
                    Name = vacationType.Name,
                    //DefaultType = vacationType.DefaultType,
                    IsActive = vacationType.IsActive,
                    NameTranslations = vacationType.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            return VacationsType;

        }
        public async Task<bool> Delete(int VacationsTypeId)
        {
            var vacationsType = await repositoryManager.DefaultVacationTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == VacationsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);
            vacationsType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int vacationTypeId)
        {
            var vacationType = await repositoryManager.DefaultVacationTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == vacationTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);
            vacationType.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultVacationTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
