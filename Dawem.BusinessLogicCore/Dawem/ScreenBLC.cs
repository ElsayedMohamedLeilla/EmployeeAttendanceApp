using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
using Dawem.Models.Response.AdminPanel.Subscriptions.Screens;
using Dawem.Models.Response.Dawem.Others;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.Subscriptions
{
    public class ScreenBLC : IScreenBLC
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IScreenBLValidation screenBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public ScreenBLC(IUnitOfWork<ApplicationDBContext> _unitOfWork,
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
        public async Task<GetAllScreensWithAvailableActionsResponse> GetAllScreensWithAvailableActions()
        {
            var screenRepository = repositoryManager.ScreenRepository;

            var criteria = new GetScreensCriteria
            {
                IsActive = true,
            };
            var query = screenRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = screenRepository.OrderBy(query, nameof(Screen.Type), LeillaKeys.Asc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var screensTypes = await queryPaged.GroupBy(s => s.Type).Select(screenGroup => new GetAllScreensWithAvailableActionsResponseModel
            {
                Type = screenGroup.Key,
                TypeName = TranslationHelper.GetTranslation(screenGroup.Key.ToString() + nameof(AuthenticationType), requestInfo.Lang),
                Screens = screenGroup.OrderBy(s=>s.Order).Select(s => new ScreenWithAvailableActionsDTO
                {
                    ScreenId = s.Id,
                    ScreenName = s.ScreenNameTranslations.Any(p => p.Language.ISO2 == requestInfo.Lang) ?
                    s.ScreenNameTranslations.First(p => p.Language.ISO2 == requestInfo.Lang).Name : null,
                    AvailableActions = s.ScreenActions.Any() ? s.ScreenActions.Select(a => a.ActionCode).ToList() : null,
                }).ToList()
            }).ToListAsync();

            return new GetAllScreensWithAvailableActionsResponse
            {
                ScreensTypes = screensTypes
            };

            #endregion

        }
    }
}

