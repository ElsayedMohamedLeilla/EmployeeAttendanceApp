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
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultPenalties;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultPenaltiesBL : IDefaultPenaltiesBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultPenaltiesBLValidation PenaltiesBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultPenaltiesBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultPenaltiesBLValidation _PenaltiesBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            PenaltiesBLValidation = _PenaltiesBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultPenaltiesDTO model)
        {
            #region Business Validation

            await PenaltiesBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Penalties

            #region Set Penalties code

            var getNextCode = await repositoryManager.DefaultPenaltiesRepository
                .Get(e => e.LookupType == LookupsType.Penalties)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var Penalties = mapper.Map<DefaultLookup>(model);
            Penalties.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            Penalties.LookupType = LookupsType.Penalties;
            Penalties.AddUserId = requestInfo.UserId;
            Penalties.Code = getNextCode;
            repositoryManager.DefaultPenaltiesRepository.Insert(Penalties);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return Penalties.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultPenaltiesDTO model)
        {
            #region Business Validation

            await PenaltiesBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Penalties
            var getPenalties = await repositoryManager.DefaultPenaltiesRepository.GetByIdAsync(model.Id);
            getPenalties.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getPenalties.DefaultType = model.DefaultType;
            getPenalties.IsActive = model.IsActive;
            getPenalties.ModifiedDate = DateTime.Now;
            getPenalties.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getPenalties.Id)
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


        public async Task<GetDefaultPenaltiesResponseDTO> Get(GetDefaultPenaltiesCriteria criteria)
        {
            var PenaltiesRepository = repositoryManager.DefaultPenaltiesRepository;
            var query = PenaltiesRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = PenaltiesRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var PenaltiesList = await queryPaged.Select(Penalties => new GetDefaultPenaltiesResponseModelDTO
            {
                Id = Penalties.Id,
                Code = Penalties.Code,
                Name = Penalties.Name,
                //DefaultType = Penalties.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(Penalties.DefaultType.ToString(), requestInfo.Lang),
                IsActive = Penalties.IsActive,
            }).ToListAsync();

            return new GetDefaultPenaltiesResponseDTO
            {
                DefaultPenalties = PenaltiesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultPenaltiesDropDownResponseDTO> GetForDropDown(GetDefaultPenaltiesCriteria criteria)
        {
            criteria.IsActive = true;
            var PenaltiesRepository = repositoryManager.DefaultPenaltiesRepository;
            var query = PenaltiesRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = PenaltiesRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var PenaltiesList = await queryPaged.Select(e => new GetDefaultPenaltiesForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultPenaltiesDropDownResponseDTO
            {
                DefaultPenalties = PenaltiesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultPenaltiesInfoResponseDTO> GetInfo(int PenaltiesId)
        {
            var Penalties = await repositoryManager.DefaultPenaltiesRepository.Get(e => e.LookupType == LookupsType.Penalties && e.Id == PenaltiesId && !e.IsDeleted)
                .Select(Penalties => new GetDefaultPenaltiesInfoResponseDTO
                {
                    Code = Penalties.Code,
                    Name = Penalties.Name,
                    //DefaultType = Penalties.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(Penalties.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = Penalties.IsActive,
                    NameTranslations = Penalties.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryPenaltyNotFound);

            return Penalties;
        }
        public async Task<GetDefaultPenaltiesByIdResponseDTO> GetById(int PenaltiesId)
        {
            var Penalties = await repositoryManager.DefaultPenaltiesRepository.Get(e => e.LookupType == LookupsType.Penalties && e.Id == PenaltiesId && !e.IsDeleted)
                .Select(Penalties => new GetDefaultPenaltiesByIdResponseDTO
                {
                    Id = Penalties.Id,
                    Code = Penalties.Code,
                    Name = Penalties.Name,
                    //DefaultType = Penalties.DefaultType,
                    IsActive = Penalties.IsActive,
                    NameTranslations = Penalties.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(AmgadKeys.SorryPenaltyNotFound);

            return Penalties;

        }
        public async Task<bool> Delete(int PenaltiesId)
        {
            var Penalties = await repositoryManager.DefaultPenaltiesRepository.GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.Penalties && d.Id == PenaltiesId) ??
                throw new BusinessValidationException(AmgadKeys.SorryPenaltyNotFound);
            Penalties.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int PenaltiesId)
        {
            var Penalties = await repositoryManager.DefaultPenaltiesRepository.GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.Penalties && !d.IsActive && d.Id == PenaltiesId) ??
                throw new BusinessValidationException(AmgadKeys.SorryPenaltyNotFound);
            Penalties.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultPenaltiesRepository.GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.Penalties && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(AmgadKeys.SorryPenaltyNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
