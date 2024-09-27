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
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultShiftsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultShiftTypeBL : IDefaultShiftTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultShiftTypeBLValidation ShiftTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultShiftTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultShiftTypeBLValidation _ShiftsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            ShiftTypeBLValidation = _ShiftsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultShiftsTypeDTO model)
        {
            #region Business Validation

            await ShiftTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert ShiftsType

            #region Set ShiftsType code

            var getNextCode = await repositoryManager.DefaultShiftTypeRepository
                .Get(e => e.LookupType == LookupsType.ShiftsTypes)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var ShiftType = mapper.Map<DefaultLookup>(model);
            ShiftType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            ShiftType.LookupType = LookupsType.ShiftsTypes;
            ShiftType.AddUserId = requestInfo.UserId;
            ShiftType.Code = getNextCode;
            repositoryManager.DefaultShiftTypeRepository.Insert(ShiftType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return ShiftType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultShiftsTypeDTO model)
        {
            #region Business Validation

            await ShiftTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update ShiftsType
            var getShiftsType = await repositoryManager.DefaultShiftTypeRepository.GetByIdAsync(model.Id);
            getShiftsType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getShiftsType.DefaultType = model.DefaultType;
            getShiftsType.IsActive = model.IsActive;
            getShiftsType.ModifiedDate = DateTime.Now;
            getShiftsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getShiftsType.Id)
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


        public async Task<GetDefaultShiftsTypeResponseDTO> Get(GetDefaultShiftTypeCriteria criteria)
        {
            var ShiftsTypeRepository = repositoryManager.DefaultShiftTypeRepository;
            var query = ShiftsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ShiftsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var ShiftsTypesList = await queryPaged.Select(ShiftType => new GetDefaultShiftsTypeResponseModelDTO
            {
                Id = ShiftType.Id,
                Code = ShiftType.Code,
                Name = ShiftType.Name,
                //DefaultType = ShiftType.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(ShiftType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = ShiftType.IsActive,
            }).ToListAsync();

            return new GetDefaultShiftsTypeResponseDTO
            {
                DefaultShiftsTypes = ShiftsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultShiftsTypeDropDownResponseDTO> GetForDropDown(GetDefaultShiftTypeCriteria criteria)
        {
            criteria.IsActive = true;
            var ShiftsTypeRepository = repositoryManager.DefaultShiftTypeRepository;
            var query = ShiftsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = ShiftsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var ShiftsTypesList = await queryPaged.Select(e => new GetDefaultShiftsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultShiftsTypeDropDownResponseDTO
            {
                DefaultShiftsTypes = ShiftsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultShiftsTypeInfoResponseDTO> GetInfo(int ShiftsTypeId)
        {
            var ShiftsType = await repositoryManager.DefaultShiftTypeRepository.Get(e => e.Id == ShiftsTypeId && !e.IsDeleted)
                .Select(ShiftType => new GetDefaultShiftsTypeInfoResponseDTO
                {
                    Code = ShiftType.Code,
                    Name = ShiftType.Name,
                    //DefaultType = ShiftType.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(ShiftType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = ShiftType.IsActive,
                    NameTranslations = ShiftType.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryShiftTypeNotFound);

            return ShiftsType;
        }
        public async Task<GetDefaultShiftsTypeByIdResponseDTO> GetById(int ShiftsTypeId)
        {
            var ShiftsType = await repositoryManager.DefaultShiftTypeRepository.Get(e => e.Id == ShiftsTypeId && !e.IsDeleted)
                .Select(ShiftType => new GetDefaultShiftsTypeByIdResponseDTO
                {
                    Id = ShiftType.Id,
                    Code = ShiftType.Code,
                    Name = ShiftType.Name,
                    //DefaultType = ShiftType.DefaultType,
                    IsActive = ShiftType.IsActive,
                    NameTranslations = ShiftType.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryShiftTypeNotFound);

            return ShiftsType;

        }
        public async Task<bool> Delete(int ShiftsTypeId)
        {
            var ShiftsType = await repositoryManager.DefaultShiftTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == ShiftsTypeId) ??
                throw new BusinessValidationException(AmgadKeys.SorryShiftTypeNotFound);
            ShiftsType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int ShiftTypeId)
        {
            var ShiftType = await repositoryManager.DefaultShiftTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == ShiftTypeId) ??
                throw new BusinessValidationException(AmgadKeys.SorryShiftTypeNotFound);
            ShiftType.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultShiftTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryShiftTypeNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
