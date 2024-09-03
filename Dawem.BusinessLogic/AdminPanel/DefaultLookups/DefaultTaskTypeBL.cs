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
using Dawem.Models.Dtos.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Response.AdminPanel.DefaultLookups.DefaultTasksTypes;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.DefaultLookups
{
    public class DefaultTaskTypeBL : IDefaultTaskTypeBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IDefaultTaskTypeBLValidation TaskTypeBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;


        public DefaultTaskTypeBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
         IRepositoryManager _repositoryManager,
         IMapper _mapper,
        RequestInfo _requestHeaderContext,
        IDefaultTaskTypeBLValidation _TasksTypeBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            TaskTypeBLValidation = _TasksTypeBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateDefaultTasksTypeDTO model)
        {
            #region Business Validation

            await TaskTypeBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert TasksType

            #region Set TasksType code

            var getNextCode = await repositoryManager.DefaultTaskTypeRepository
                .Get(e => e.LookupType == LookupsType.TasksTypes)
                .Select(e => e.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var TaskType = mapper.Map<DefaultLookup>(model);
            TaskType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default
            TaskType.LookupType = LookupsType.TasksTypes;
            TaskType.AddUserId = requestInfo.UserId;
            TaskType.Code = getNextCode;
            repositoryManager.DefaultTaskTypeRepository.Insert(TaskType);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return TaskType.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateDefaultTasksTypeDTO model)
        {
            #region Business Validation

            await TaskTypeBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update TasksType
            var getTasksType = await repositoryManager.DefaultTaskTypeRepository.GetByIdAsync(model.Id);
            getTasksType.Name = model.NameTranslations.FirstOrDefault().Name; // first Language is the default;
            //getTasksType.DefaultType = model.DefaultType;
            getTasksType.IsActive = model.IsActive;
            getTasksType.ModifiedDate = DateTime.Now;
            getTasksType.ModifyUserId = requestInfo.UserId;
            await unitOfWork.SaveAsync();
            #endregion

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.DefaultLookupsNameTranslationRepository
                    .Get(e => e.DefaultLookupId == getTasksType.Id)
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


        public async Task<GetDefaultTasksTypeResponseDTO> Get(GetDefaultTaskTypeCriteria criteria)
        {
            var TasksTypeRepository = repositoryManager.DefaultTaskTypeRepository;
            var query = TasksTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = TasksTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var TasksTypesList = await queryPaged.Select(TaskType => new GetDefaultTasksTypeResponseModelDTO
            {
                Id = TaskType.Id,
                Code = TaskType.Code,
                Name = TaskType.Name,
                //DefaultType = TaskType.DefaultType,
                // DefaultTypeName = TranslationHelper.GetTranslation(TaskType.DefaultType.ToString(), requestInfo.Lang),
                IsActive = TaskType.IsActive,
            }).ToListAsync();

            return new GetDefaultTasksTypeResponseDTO
            {
                DefaultTasksTypes = TasksTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }


        public async Task<GetDefaultTasksTypeDropDownResponseDTO> GetForDropDown(GetDefaultTaskTypeCriteria criteria)
        {
            criteria.IsActive = true;
            var TasksTypeRepository = repositoryManager.DefaultTaskTypeRepository;
            var query = TasksTypeRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = TasksTypeRepository.OrderBy(query, nameof(DefaultLookup.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var TasksTypesList = await queryPaged.Select(e => new GetDefaultTasksTypeForDropDownResponseModelDTO
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();

            return new GetDefaultTasksTypeDropDownResponseDTO
            {
                DefaultTasksTypes = TasksTypesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetDefaultTasksTypeInfoResponseDTO> GetInfo(int TasksTypeId)
        {
            var TasksType = await repositoryManager.DefaultTaskTypeRepository.Get(e => e.Id == TasksTypeId && !e.IsDeleted)
                .Select(TaskType => new GetDefaultTasksTypeInfoResponseDTO
                {
                    Code = TaskType.Code,
                    Name = TaskType.Name,
                    //DefaultType = TaskType.DefaultType,
                    //DefaultTypeName = TranslationHelper.GetTranslation(TaskType.DefaultType.ToString(), requestInfo.Lang),
                    IsActive = TaskType.IsActive,
                    NameTranslations = TaskType.NameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNotFound);

            return TasksType;
        }
        public async Task<GetDefaultTasksTypeByIdResponseDTO> GetById(int TasksTypeId)
        {
            var TasksType = await repositoryManager.DefaultTaskTypeRepository.Get(e => e.Id == TasksTypeId && !e.IsDeleted)
                .Select(TaskType => new GetDefaultTasksTypeByIdResponseDTO
                {
                    Id = TaskType.Id,
                    Code = TaskType.Code,
                    Name = TaskType.Name,
                    //DefaultType = TaskType.DefaultType,
                    IsActive = TaskType.IsActive,
                    NameTranslations = TaskType.NameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()

                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNotFound);

            return TasksType;

        }
        public async Task<bool> Delete(int TasksTypeId)
        {
            var TasksType = await repositoryManager.DefaultTaskTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == TasksTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNotFound);
            TasksType.Delete();

            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> Enable(int TaskTypeId)
        {
            var TaskType = await repositoryManager.DefaultTaskTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive && d.Id == TaskTypeId) ??
                throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNotFound);
            TaskType.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var group = await repositoryManager.DefaultTaskTypeRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryTaskTypeNotFound);
            group.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
    }
}
