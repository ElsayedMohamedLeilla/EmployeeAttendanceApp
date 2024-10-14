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
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultJobTitles;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultJobTitlesBL : IDefaultJobTitlesBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultJobTitlesBLValidation JobTitlesBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultJobTitlesBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultJobTitlesBLValidation _JobTitlesBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            JobTitlesBLValidation = _JobTitlesBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultJobTitlesDTO model)
        {
            #region Business Validation

            await JobTitlesBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert JobTitles

            #region Set JobTitles code

            var getNextCode = await repositoryManager.DefaultJobTitlesRepository
                .Get(e => e.LookupType == LookupsType.JobTitles)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var JobTitles = mapper.Map<DefaultLookup>(model);
            JobTitles.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            JobTitles.LookupType = LookupsType.JobTitles;
            JobTitles.AddUserId = requestInfo.UserId;
            JobTitles.Code = getNextCode;
            repositoryManager.DefaultJobTitlesRepository.Insert(JobTitles);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return JobTitles.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultJobTitlesDTO model)
        {
            #region Business Validation

            await JobTitlesBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update JobTitles
            var getJobTitles = await repositoryManager.DefaultJobTitlesRepository.GetByIdAsync(model.Id);
            getJobTitles.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getJobTitles.DefaultType = model.DefaultType;
            getJobTitles.IsActive = model.IsActive;
            getJobTitles.ModifiedDate = DateTime.Now;
            getJobTitles.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getJobTitles.Id)
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


        public async Task<GetDefaultJobTitlesResponseDTO> Get(GetDefaultJobTitlesCriteria criteria)
        {
            var JobTitlesRepository = repositoryManager.DefaultJobTitlesRepository;
            var query = JobTitlesRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = JobTitlesRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var JobTitlesList = await queryPaged.Select(JobTitles => new GetDefaultJobTitlesResponseModelDTO
            {
                Id = JobTitles.Id,
                Code = JobTitles.Code,
                Name = JobTitles.Name,
                //DefaultType = JobTitles.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(JobTitles.DefaultType.ToString(), requestInfo.Lang),
                IsActive = JobTitles.IsActive,
            }).ToListAsync();

            return new GetDefaultJobTitlesResponseDTO
            {
                DefaultJobTitles = JobTitlesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultJobTitlesDropDownResponseDTO> GetForDropDown(GetDefaultJobTitlesCriteria criteria)
        {
            criteria.IsActive = true;
            var JobTitlesRepository = repositoryManager.DefaultJobTitlesRepository;
            var query = JobTitlesRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = JobTitlesRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var JobTitlesList = await queryPaged.Select(e => new GetDefaultJobTitlesForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultJobTitlesDropDownResponseDTO
            {
                DefaultJobTitles = JobTitlesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultJobTitlesInfoResponseDTO> GetInfo(int JobTitlesId)
        {
            var JobTitles = await repositoryManager.DefaultJobTitlesRepository.Get(e => e.LookupType == LookupsType.JobTitles && e.Id == JobTitlesId && !e.IsDeleted)
                .Select(JobTitles => new GetDefaultJobTitlesInfoResponseDTO
                {
                    Code = JobTitles.Code,
                    Name = JobTitles.Name,
                    IsActive = JobTitles.IsActive,
                    NameTranslations = JobTitles.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);

            return JobTitles;
        }
        public async Task<GetDefaultJobTitlesByIdResponseDTO> GetById(int JobTitlesId)
        {
            var JobTitles = await repositoryManager.DefaultJobTitlesRepository.Get(e => e.LookupType == LookupsType.JobTitles && e.Id == JobTitlesId && !e.IsDeleted)
                .Select(JobTitles => new GetDefaultJobTitlesByIdResponseDTO
                {
                    Id = JobTitles.Id,
                    Code = JobTitles.Code,
                    Name = JobTitles.Name,
                    //DefaultType = JobTitles.DefaultType,
                    IsActive = JobTitles.IsActive,
                    NameTranslations = JobTitles.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);

            return JobTitles;

        }
        public async Task<bool> Delete(int JobTitlesId)
        {
            var JobTitles = await repositoryManager.DefaultJobTitlesRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.JobTitles && !d.IsDeleted && d.Id == JobTitlesId) ??
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);
            JobTitles.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int JobTitlesId)
        {
            var JobTitles = await repositoryManager.DefaultJobTitlesRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.JobTitles && !d.IsDeleted && !d.IsActive && d.Id == JobTitlesId) ??
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);
            JobTitles.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultJobTitlesRepository.
                GetEntityByConditionWithTrackingAsync(d => d.LookupType == LookupsType.JobTitles && !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryJobTitleNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
