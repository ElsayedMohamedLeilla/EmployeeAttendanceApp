using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Employees.Employees;
using Dawem.Models.Dtos.Dawem.Shared;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;
using Dawem.Models.Response.AdminPanel.Subscriptions.Screens;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.Subscriptions
{
    public class ScreenGroupBL : IScreenGroupBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IScreenGroupBLValidation screenGroupBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public ScreenGroupBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IScreenGroupBLValidation _screenGroupBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            screenGroupBLValidation = _screenGroupBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateScreenGroupModel model)
        {
            #region Business Validation

            await screenGroupBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert ScreenGroup

            var screenGroup = mapper.Map<ScreenGroup>(model);
            screenGroup.AddUserId = requestInfo.UserId;
            repositoryManager.ScreenGroupRepository.Insert(screenGroup);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return screenGroup.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateScreenGroupModel model)
        {
            #region Business Validation

            await screenGroupBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update ScreenGroup

            var getScreenGroup = await repositoryManager.ScreenGroupRepository.GetEntityByConditionWithTrackingAsync(screenGroup => !screenGroup.IsDeleted
            && screenGroup.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryScreenGroupNotFound);

            getScreenGroup.IsActive = model.IsActive;
            getScreenGroup.ModifiedDate = DateTime.Now;
            getScreenGroup.ModifyUserId = requestInfo.UserId;
            getScreenGroup.Notes = model.Notes;
            getScreenGroup.Icon = model.Icon;

            await unitOfWork.SaveAsync();

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.ScreenGroupNameTranslationRepository
                    .Get(e => e.ScreenGroupId == getScreenGroup.Id)
                    .ToListAsync();

            var existingNameTranslationsIds = exisNameTranslationsDbList.Select(e => e.Id)
                .ToList();

            var addedScreenGroupNameTranslations = model.NameTranslations != null ? model.NameTranslations
                .Where(ge => !existingNameTranslationsIds.Contains(ge.Id))
                .Select(ge => new ScreenGroupNameTranslation
                {
                    ScreenGroupId = model.Id,
                    LanguageId = ge.LanguageId,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<ScreenGroupNameTranslation>();

            var removedScreenGroupNameTranslationsIds = exisNameTranslationsDbList
                .Where(ge => model.NameTranslations == null || !model.NameTranslations.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedScreenGroupNameTranslations = exisNameTranslationsDbList
                .Where(e => removedScreenGroupNameTranslationsIds.Contains(e.Id))
                .ToList();

            var updatedScreenGroupNameTranslations = exisNameTranslationsDbList.
                Where(nt => model.NameTranslations != null && model.NameTranslations.
                Any(mi => mi.Id == nt.Id && (mi.Name != nt.Name || mi.LanguageId != nt.LanguageId))).
                ToList();

            if (removedScreenGroupNameTranslations.Count() > 0)
                repositoryManager.ScreenGroupNameTranslationRepository.BulkDeleteIfExist(removedScreenGroupNameTranslations);
            if (addedScreenGroupNameTranslations.Count() > 0)
                repositoryManager.ScreenGroupNameTranslationRepository.BulkInsert(addedScreenGroupNameTranslations);
            if (updatedScreenGroupNameTranslations.Count() > 0)
            {
                updatedScreenGroupNameTranslations.ForEach(i =>
                {
                    i.Name = model.NameTranslations.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                    i.LanguageId = model.NameTranslations.FirstOrDefault(mi => mi.Id == i.Id)?.LanguageId ?? 0;
                });
                repositoryManager.ScreenGroupNameTranslationRepository.BulkUpdate(updatedScreenGroupNameTranslations);
            }

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetScreenGroupsResponse> Get(GetScreenGroupsCriteria criteria)
        {
            var screenGroupRepository = repositoryManager.ScreenGroupRepository;
            var query = screenGroupRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = screenGroupRepository.OrderBy(query, nameof(ScreenGroup.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var screenGroupsList = await queryPaged.Select(screenGroup => new GetScreenGroupResponseModel
            {
                Id = screenGroup.Id,
                Name = screenGroup.ScreenGroupNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                GroupType = screenGroup.GroupType,
                GroupTypeName = TranslationHelper.GetTranslation(screenGroup.GroupTypeName.ToString() + nameof(ScreenGroupType), requestInfo.Lang),
                IsActive = screenGroup.IsActive,
            }).ToListAsync();
            return new GetScreenGroupsResponse
            {
                ScreenGroups = screenGroupsList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetScreenGroupsForDropDownResponse> GetForDropDown(GetScreenGroupsCriteria criteria)
        {
            criteria.IsActive = true;
            var screenGroupRepository = repositoryManager.ScreenGroupRepository;
            var query = screenGroupRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = screenGroupRepository.OrderBy(query, nameof(ScreenGroup.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var screenGroupsList = await queryPaged.Select(screenGroup => new GetScreenGroupsForDropDownResponseModel
            {
                Id = screenGroup.Id,
                Name = screenGroup.ScreenGroupNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
            }).ToListAsync();

            return new GetScreenGroupsForDropDownResponse
            {
                ScreenGroups = screenGroupsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetScreenGroupInfoResponseModel> GetInfo(int screenGroupId)
        {
            var screenGroup = await repositoryManager.ScreenGroupRepository.Get(e => e.Id == screenGroupId && !e.IsDeleted)
                .Select(screenGroup => new GetScreenGroupInfoResponseModel
                {
                    Name = screenGroup.ScreenGroupNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    IsActive = screenGroup.IsActive,
                    Notes = screenGroup.Notes,
                    Icon = screenGroup.Icon,
                    NameTranslations = screenGroup.ScreenGroupNameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScreenGroupNotFound);

            return screenGroup;
        }
        public async Task<GetScreenGroupByIdResponseModel> GetById(int screenGroupId)
        {
            var screenGroup = await repositoryManager.ScreenGroupRepository.Get(e => e.Id == screenGroupId && !e.IsDeleted)
                .Select(screenGroup => new GetScreenGroupByIdResponseModel
                {
                    Id = screenGroup.Id,
                    Name = screenGroup.ScreenGroupNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    IsActive = screenGroup.IsActive,
                    Notes = screenGroup.Notes,
                    Icon = screenGroup.Icon,
                    NameTranslations = screenGroup.ScreenGroupNameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScreenGroupNotFound);

            return screenGroup;

        }
        public async Task<bool> Enable(int screenGroupId)
        {
            var responsibility = await repositoryManager.ScreenGroupRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive
                && d.Id == screenGroupId) ??
                throw new BusinessValidationException(LeillaKeys.SorryScreenGroupNotFound);
            responsibility.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var responsibility = await repositoryManager.ScreenGroupRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted
                && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryScreenGroupNotFound);
            responsibility.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int screenGroupd)
        {
            var screenGroup = await repositoryManager.ScreenGroupRepository.
                GetEntityByConditionWithTrackingAsync(screenGroup => !screenGroup.IsDeleted && screenGroup.Id == screenGroupd ) ??
                throw new BusinessValidationException(LeillaKeys.SorryScreenGroupNotFound);
            screenGroup.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetScreenGroupsInformationsResponseDTO> GetScreenGroupsInformations()
        {
            var screenGroupRepository = repositoryManager.ScreenGroupRepository;
            var query = screenGroupRepository.Get();

            #region Handle Response

            return new GetScreenGroupsInformationsResponseDTO
            {
                TotalCount = await query.Where(screenGroup => !screenGroup.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(screenGroup => !screenGroup.IsDeleted && screenGroup.IsActive).CountAsync(),
                NotActiveCount = await query.Where(screenGroup => !screenGroup.IsDeleted && !screenGroup.IsActive).CountAsync(),
                DeletedCount = await query.Where(screenGroup => screenGroup.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

