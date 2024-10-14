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
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultJustificationsTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultJustificationTypeBL : IDefaultJustificationTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultJustificationTypeBLValidation JustificationTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultJustificationTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultJustificationTypeBLValidation _JustificationsTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            JustificationTypeBLValidation = _JustificationsTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultJustificationsTypeDTO model)
        {
            #region Business Validation

            await JustificationTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert JustificationsType

            #region Set JustificationsType code

            var getNextCode = await repositoryManager.DefaultJustificationTypeRepository
                .Get(e => e.LookupType == LookupsType.JustificationsTypes)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var JustificationType = mapper.Map<DefaultLookup>(model);
            JustificationType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            JustificationType.LookupType = LookupsType.JustificationsTypes;
            JustificationType.AddUserId = requestInfo.UserId;
            JustificationType.Code = getNextCode;
            repositoryManager.DefaultJustificationTypeRepository.Insert(JustificationType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return JustificationType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultJustificationsTypeDTO model)
        {
            #region Business Validation

            await JustificationTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update JustificationsType
            var getJustificationsType = await repositoryManager.DefaultJustificationTypeRepository.GetByIdAsync(model.Id);
            getJustificationsType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getJustificationsType.DefaultType = model.DefaultType;
            getJustificationsType.IsActive = model.IsActive;
            getJustificationsType.ModifiedDate = DateTime.Now;
            getJustificationsType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getJustificationsType.Id)
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


        public async Task<GetDefaultJustificationsTypeResponseDTO> Get(GetDefaultJustificationTypeCriteria criteria)
        {
            var JustificationsTypeRepository = repositoryManager.DefaultJustificationTypeRepository;
            var query = JustificationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = JustificationsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var JustificationsTypesList = await queryPaged.Select(JustificationType => new GetDefaultJustificationsTypeResponseModelDTO
            {
                Id = JustificationType.Id,
                Code = JustificationType.Code,
                Name = JustificationType.Name,
                //DefaultType = JustificationType.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(JustificationType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = JustificationType.IsActive,
            }).ToListAsync();

            return new GetDefaultJustificationsTypeResponseDTO
            {
                DefaultJustificationsTypes = JustificationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultJustificationsTypeDropDownResponseDTO> GetForDropDown(GetDefaultJustificationTypeCriteria criteria)
        {
            criteria.IsActive = true;
            var JustificationsTypeRepository = repositoryManager.DefaultJustificationTypeRepository;
            var query = JustificationsTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = JustificationsTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var JustificationsTypesList = await queryPaged.Select(e => new GetDefaultJustificationsTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultJustificationsTypeDropDownResponseDTO
            {
                DefaultJustificationsTypes = JustificationsTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultJustificationsTypeInfoResponseDTO> GetInfo(int JustificationsTypeId)
        {
            var JustificationsType = await repositoryManager.DefaultJustificationTypeRepository.Get(e => e.LookupType == LookupsType.JustificationsTypes && e.Id == JustificationsTypeId && !e.IsDeleted)
                .Select(JustificationType => new GetDefaultJustificationsTypeInfoResponseDTO
                {
                    Code = JustificationType.Code,
                    Name = JustificationType.Name,
                    //DefaultType = JustificationType.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(JustificationType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = JustificationType.IsActive,
                    NameTranslations = JustificationType.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);

            return JustificationsType;
        }
        public async Task<GetDefaultJustificationsTypeByIdResponseDTO> GetById(int JustificationsTypeId)
        {
            var JustificationsType = await repositoryManager.DefaultJustificationTypeRepository.Get(e => e.LookupType == LookupsType.JustificationsTypes && e.Id == JustificationsTypeId && !e.IsDeleted)
                .Select(JustificationType => new GetDefaultJustificationsTypeByIdResponseDTO
                {
                    Id = JustificationType.Id,
                    Code = JustificationType.Code,
                    Name = JustificationType.Name,
                    //DefaultType = JustificationType.DefaultType,
                    IsActive = JustificationType.IsActive,
                    NameTranslations = JustificationType.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);

            return JustificationsType;

        }
        public async Task<bool> Delete(int JustificationsTypeId)
        {
            var JustificationsType = await repositoryManager.DefaultJustificationTypeRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.JustificationsTypes && d.Id == JustificationsTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);
            JustificationsType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int JustificationTypeId)
        {
            var JustificationType = await repositoryManager.DefaultJustificationTypeRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.JustificationsTypes && !d.IsActive && d.Id == JustificationTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);
            JustificationType.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultJustificationTypeRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.JustificationsTypes && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryJustificationsTypeNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
