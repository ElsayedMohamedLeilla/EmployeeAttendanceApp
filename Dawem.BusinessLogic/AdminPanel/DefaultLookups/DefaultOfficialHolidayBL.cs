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
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultOfficialHolidaysTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultOfficialHolidayBL : IDefaultOfficialHolidayBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultOfficialHolidayBLValidation OfficialHolidayBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultOfficialHolidayBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultOfficialHolidayBLValidation _OfficialHolidaysBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            OfficialHolidayBLValidation = _OfficialHolidaysBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultOfficialHolidaysDTO model)
        {
            #region Business Validation

            await OfficialHolidayBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert OfficialHolidaysType

            #region Set OfficialHolidaysType code

            var getNextCode = await repositoryManager.DefaultOfficialHolidayRepository
                .Get(e => e.LookupType == LookupsType.OfficialHoliday)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var OfficialHolidayType = mapper.Map<DefaultLookup>(model);
            OfficialHolidayType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            OfficialHolidayType.LookupType = LookupsType.OfficialHoliday;
            OfficialHolidayType.AddUserId = requestInfo.UserId;
            OfficialHolidayType.Code = getNextCode;
            repositoryManager.DefaultOfficialHolidayRepository.Insert(OfficialHolidayType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return OfficialHolidayType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultOfficialHolidaysDTO model)
        {
            #region Business Validation

            await OfficialHolidayBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update OfficialHolidaysType
            var getOfficialHolidaysType = await repositoryManager.DefaultOfficialHolidayRepository.GetByIdAsync(model.Id);
            getOfficialHolidaysType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getOfficialHolidaysType.DefaultType = model.DefaultType;
            getOfficialHolidaysType.IsActive = model.IsActive;
            getOfficialHolidaysType.ModifiedDate = DateTime.Now;
            getOfficialHolidaysType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getOfficialHolidaysType.Id)
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


        public async Task<GetDefaultOfficialHolidaysResponseDTO> Get(GetDefaultOfficialHolidayCriteria criteria)
        {
            var OfficialHolidaysRepository = repositoryManager.DefaultOfficialHolidayRepository;
            var query = OfficialHolidaysRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = OfficialHolidaysRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var OfficialHolidaysTypesList = await queryPaged.Select(OfficialHolidayType => new GetDefaultOfficialHolidaysTypeResponseModelDTO
            {
                Id = OfficialHolidayType.Id,
                Code = OfficialHolidayType.Code,
                Name = OfficialHolidayType.Name,
                //DefaultType = OfficialHolidayType.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(OfficialHolidayType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = OfficialHolidayType.IsActive,
            }).ToListAsync();

            return new GetDefaultOfficialHolidaysResponseDTO
            {
                DefaultOfficialHolidaysTypes = OfficialHolidaysTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultOfficialHolidaysDropDownResponseDTO> GetForDropDown(GetDefaultOfficialHolidayCriteria criteria)
        {
            criteria.IsActive = true;
            var OfficialHolidaysTypeRepository = repositoryManager.DefaultOfficialHolidayRepository;
            var query = OfficialHolidaysTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = OfficialHolidaysTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var OfficialHolidaysTypesList = await queryPaged.Select(e => new GetDefaultOfficialHolidaysTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultOfficialHolidaysDropDownResponseDTO
            {
                DefaultOfficialHolidaysTypes = OfficialHolidaysTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultOfficialHolidaysInfoResponseDTO> GetInfo(int OfficialHolidaysTypeId)
        {
            var officialHolidaysType = await repositoryManager.DefaultOfficialHolidayRepository.Get(e => e.Id == OfficialHolidaysTypeId && !e.IsDeleted)
                .Select(OfficialHolidayType => new GetDefaultOfficialHolidaysInfoResponseDTO
                {
                    Code = OfficialHolidayType.Code,
                    Name = OfficialHolidayType.Name,
                    //DefaultType = OfficialHolidayType.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(OfficialHolidayType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = OfficialHolidayType.IsActive,
                    NameTranslations = OfficialHolidayType.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);

            return officialHolidaysType;
        }
        public async Task<GetDefaultOfficialHolidaysByIdResponseDTO> GetById(int officialHolidaysId)
        {
            var OfficialHolidaysType = await repositoryManager.DefaultOfficialHolidayRepository.Get(e => e.Id == officialHolidaysId && !e.IsDeleted)
                .Select(OfficialHolidayType => new GetDefaultOfficialHolidaysByIdResponseDTO
                {
                    Id = OfficialHolidayType.Id,
                    Code = OfficialHolidayType.Code,
                    Name = OfficialHolidayType.Name,
                    //DefaultType = OfficialHolidayType.DefaultType,
                    IsActive = OfficialHolidayType.IsActive,
                    NameTranslations = OfficialHolidayType.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);

            return OfficialHolidaysType;

        }
        public async Task<bool> Delete(int OfficialHolidaysTypeId)
        {
            var OfficialHolidaysType = await repositoryManager.DefaultOfficialHolidayRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == OfficialHolidaysTypeId) ??
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);
            OfficialHolidaysType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int OfficialHolidayTypeId)
        {
            var OfficialHolidayType = await repositoryManager.DefaultOfficialHolidayRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == OfficialHolidayTypeId) ??
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);
            OfficialHolidayType.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultOfficialHolidayRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryHolidayNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
