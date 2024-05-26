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
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Response.AdminPanel.Subscriptions.Screens;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.Subscriptions
{
    public class ScreenBL : IScreenBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IScreenBLValidation screenBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public ScreenBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IScreenBLValidation _screenBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            screenBLValidation = _screenBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateScreenModel model)
        {
            #region Business Validation

            await screenBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Screen

            var screen = mapper.Map<Screen>(model);
            screen.AddUserId = requestInfo.UserId;
            screen.Type = requestInfo.Type;
            repositoryManager.ScreenRepository.Insert(screen);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return screen.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateScreenModel model)
        {
            #region Business Validation

            await screenBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Screen

            var getScreen = await repositoryManager.ScreenRepository.GetEntityByConditionWithTrackingAsync(screen => !screen.IsDeleted
            && screen.Id == model.Id) ?? throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);

            getScreen.IsActive = model.IsActive;
            getScreen.ModifiedDate = DateTime.Now;
            getScreen.ModifyUserId = requestInfo.UserId;
            getScreen.Notes = model.Notes;
            getScreen.Icon = model.Icon;
            getScreen.URL = model.URL;

            await unitOfWork.SaveAsync();

            #region Handle Update Name Translations

            var exisNameTranslationsDbList = await repositoryManager.ScreenNameTranslationRepository
                    .Get(e => e.ScreenId == getScreen.Id)
                    .ToListAsync();

            var existingNameTranslationsIds = exisNameTranslationsDbList.Select(e => e.Id)
                .ToList();

            var addedScreenNameTranslations = model.NameTranslations != null ? model.NameTranslations
                .Where(ge => !existingNameTranslationsIds.Contains(ge.Id))
                .Select(ge => new ScreenNameTranslation
                {
                    ScreenId = model.Id,
                    LanguageId = ge.LanguageId,
                    Name = ge.Name,
                    ModifyUserId = requestInfo.UserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList() : new List<ScreenNameTranslation>();

            var removedScreenNameTranslationsIds = exisNameTranslationsDbList
                .Where(ge => model.NameTranslations == null || !model.NameTranslations.Select(i => i.Id).Contains(ge.Id))
                .Select(ge => ge.Id)
                .ToList();

            var removedScreenNameTranslations = exisNameTranslationsDbList
                .Where(e => removedScreenNameTranslationsIds.Contains(e.Id))
                .ToList();

            var updatedScreenNameTranslations = exisNameTranslationsDbList.
                Where(nt => model.NameTranslations != null && model.NameTranslations.
                Any(mi => mi.Id == nt.Id && (mi.Name != nt.Name || mi.LanguageId != nt.LanguageId))).
                ToList();

            if (removedScreenNameTranslations.Count() > 0)
                repositoryManager.ScreenNameTranslationRepository.BulkDeleteIfExist(removedScreenNameTranslations);
            if (addedScreenNameTranslations.Count() > 0)
                repositoryManager.ScreenNameTranslationRepository.BulkInsert(addedScreenNameTranslations);
            if (updatedScreenNameTranslations.Count() > 0)
            {
                updatedScreenNameTranslations.ForEach(i =>
                {
                    i.Name = model.NameTranslations.FirstOrDefault(mi => mi.Id == i.Id)?.Name;
                    i.LanguageId = model.NameTranslations.FirstOrDefault(mi => mi.Id == i.Id)?.LanguageId ?? 0;
                });
                repositoryManager.ScreenNameTranslationRepository.BulkUpdate(updatedScreenNameTranslations);
            }

            #endregion

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetScreensResponse> Get(GetScreensCriteria criteria)
        {
            var screenRepository = repositoryManager.ScreenRepository;
            var query = screenRepository.GetAsQueryable(criteria);

            #region paging
            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);
            #region sorting
            var queryOrdered = screenRepository.OrderBy(query, nameof(Screen.Id), LeillaKeys.Desc);
            #endregion
            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;
            #endregion

            #region Handle Response

            var screensList = await queryPaged.Select(screen => new GetScreenResponseModel
            {
                Id = screen.Id,
                Name = screen.ScreenNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                Type = screen.Type,
                TypeName = TranslationHelper.GetTranslation(screen.Type.ToString() + nameof(AuthenticationType), requestInfo.Lang),
                IsActive = screen.IsActive,
            }).ToListAsync();
            return new GetScreensResponse
            {
                Screens = screensList,
                TotalCount = await query.CountAsync()
            };
            #endregion

        }
        public async Task<GetScreensForDropDownResponse> GetForDropDown(GetScreensCriteria criteria)
        {
            criteria.IsActive = true;
            var screenRepository = repositoryManager.ScreenRepository;
            var query = screenRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = screenRepository.OrderBy(query, nameof(Screen.Id), LeillaKeys.Desc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var screensList = await queryPaged.Select(screen => new GetScreensForDropDownResponseModel
            {
                Id = screen.Id,
                Name = screen.ScreenNameTranslations.FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
            }).ToListAsync();

            return new GetScreensForDropDownResponse
            {
                Screens = screensList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetScreenInfoResponseModel> GetInfo(int screenId)
        {
            var screen = await repositoryManager.ScreenRepository.Get(e => e.Id == screenId && !e.IsDeleted)
                .Select(screen => new GetScreenInfoResponseModel
                {
                    Name = screen.ScreenNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    IsActive = screen.IsActive,
                    Notes = screen.Notes,
                    Icon = screen.Icon,
                    URL = screen.URL,
                    Actions = screen.ScreenActions != null ?
                    screen.ScreenActions.Select(a => TranslationHelper.
                    GetTranslation(a.ActionCode.ToString(), requestInfo.Lang)).
                    ToList() : null,
                    NameTranslations = screen.ScreenNameTranslations.
                    Select(pt =>
                    new NameTranslationGetInfoModel
                    {
                        Name = pt.Name,
                        LanguageName = pt.Language.NativeName
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);

            return screen;
        }
        public async Task<GetScreenByIdResponseModel> GetById(int screenId)
        {
            var screen = await repositoryManager.ScreenRepository.Get(e => e.Id == screenId && !e.IsDeleted)
                .Select(screen => new GetScreenByIdResponseModel
                {
                    Id = screen.Id,
                    Name = screen.ScreenNameTranslations.
                    FirstOrDefault(p => p.Language.ISO2 == requestInfo.Lang).Name,
                    IsActive = screen.IsActive,
                    Notes = screen.Notes,
                    Icon = screen.Icon,
                    URL = screen.URL,
                    Actions = screen.ScreenActions != null ?
                    screen.ScreenActions.Select(a => a.ActionCode).
                    ToList() : null,
                    NameTranslations = screen.ScreenNameTranslations.
                    Select(pt => new NameTranslationModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        LanguageId = pt.LanguageId
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);

            return screen;

        }
        public async Task<bool> Enable(int screenId)
        {
            var responsibility = await repositoryManager.ScreenRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && !d.IsActive
                && d.Type == requestInfo.Type && d.Id == screenId) ??
                throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);
            responsibility.Enable();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Disable(DisableModelDTO model)
        {
            var responsibility = await repositoryManager.ScreenRepository.
                GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted
                && d.Type == requestInfo.Type && d.IsActive && d.Id == model.Id) ??
                throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);
            responsibility.Disable(model.DisableReason);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> Delete(int screend)
        {
            var screen = await repositoryManager.ScreenRepository.
                GetEntityByConditionWithTrackingAsync(screen => !screen.IsDeleted && screen.Id == screend &&
                screen.Type == requestInfo.Type) ??
                throw new BusinessValidationException(LeillaKeys.SorryScreenNotFound);
            screen.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetScreensInformationsResponseDTO> GetScreensInformations()
        {
            var screenRepository = repositoryManager.ScreenRepository;
            var query = screenRepository.Get(screen => requestInfo.Type == AuthenticationType.DawemAdmin &&
            screen.Type != AuthenticationType.AdminPanel || requestInfo.Type == AuthenticationType.AdminPanel &&
            screen.Type == AuthenticationType.AdminPanel);

            #region Handle Response

            return new GetScreensInformationsResponseDTO
            {
                TotalCount = await query.Where(screen => !screen.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(screen => !screen.IsDeleted && screen.IsActive).CountAsync(),
                NotActiveCount = await query.Where(screen => !screen.IsDeleted && !screen.IsActive).CountAsync(),
                DeletedCount = await query.Where(screen => screen.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}

