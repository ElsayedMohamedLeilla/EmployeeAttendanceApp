using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Others;
using Dawem.Models.DTOs.Dawem.Screens.Screens;
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
        public async Task<GetAllScreensWithAvailableActionsResponse> GetAllScreensWithAvailableActions(GetScreensCriteria criteria)
        {
            var screenRepository = repositoryManager.MenuItemRepository;

            criteria.IsActive = true;

            var companyId = requestInfo.CompanyId;

            var getCompanyPlan = await repositoryManager.SubscriptionRepository.
                Get(s => s.CompanyId == companyId).
                Select(s => new
                {
                    s.PlanId,
                    s.Plan.AllScreensAvailable,
                }).FirstOrDefaultAsync();

            if (getCompanyPlan != null)
            {
                criteria.PlanId = getCompanyPlan.PlanId;
                criteria.IsAllScreensAvailableInPlan = getCompanyPlan.AllScreensAvailable;
            }

            IQueryable<MenuItem>? query = null;

            if (criteria.ScreensForType == ScreensForType.AllScreens)
                query = screenRepository.GetAsQueryableForGetAllScreens(criteria);

            else if (criteria.ScreensForType == ScreensForType.Menu)
                query = screenRepository.GetAsQueryableForMenu(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = screenRepository.OrderBy(query, nameof(MenuItem.AuthenticationType), LeillaKeys.Asc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var screensTypes = await queryPaged.IgnoreQueryFilters().
                GroupBy(s => s.AuthenticationType).
                Select(screenGroup => new
                {
                    Type = screenGroup.Key,
                    TypeName = TranslationHelper.GetTranslation(screenGroup.Key.ToString() + nameof(AuthenticationType), requestInfo.Lang),
                    Screens = screenGroup.OrderBy(s => s.Order).Select(screen => new ScreenDTO
                    {
                        Id = screen.Id,
                        Order = screen.Order,
                        GroupOrScreenType = screen.GroupOrScreenType,
                        ParentId = screen.ParentId,
                        Name = screen.MenuItemNameTranslations.
                        First(p => p.Language.ISO2 == requestInfo.Lang).Name,
                        Icon = screen.Icon,
                        URL = screen.URL,
                        AvailableActions = screen.MenuItemActions != null ?
                        screen.MenuItemActions.Select(a => a.ActionCode).ToList() : null,
                    }).ToList()
                }).ToListAsync();

            var response = new GetAllScreensWithAvailableActionsResponse();

            foreach (var screenType in screensTypes)
            {
                var screenTypeModel = new GetAllMenuItemsWithAvailableActionsResponseModel
                {
                    AuthenticationType = screenType.Type,
                    AuthenticationTypeName = screenType.TypeName
                };
                var masterScreens = screenType.Screens.Where(s => s.ParentId == null).OrderBy(s => s.Order);

                foreach (var screen in masterScreens)
                {
                    screenTypeModel.MenuItems.Add(new MenuItemWithAvailableActionsDTO
                    {
                        Id = screen.Id,
                        GroupOrScreenType = screen.GroupOrScreenType,
                        Name = screen.Name,
                        Icon = screen.Icon,
                        URL = screen.URL,
                        AvailableActions = screen.AvailableActions,
                        Children = HasChildren(screen.Id, screenType.Screens) ?
                        GetChildren(screen.Id, screenType.Screens) : null,
                    });
                }

                response.MenuItemsTypes.Add(screenTypeModel);

            }

            return response;

            #endregion

        }
        private bool HasChildren(int screenId, List<ScreenDTO> screens)
        {
            return screens.Any(s => s.ParentId == screenId);
        }
        private List<MenuItemWithAvailableActionsDTO> GetChildren(int screenId, List<ScreenDTO> screens)
        {
            var children = screens.Where(s => s.ParentId == screenId).ToList();

            var respnse = children.Select(screen => new MenuItemWithAvailableActionsDTO
            {
                Id = screen.Id,
                GroupOrScreenType = screen.GroupOrScreenType,
                Name = screen.Name,
                Icon = screen.Icon,
                URL = screen.URL,
                AvailableActions = screen.AvailableActions,
                Children = HasChildren(screen.Id, screens) ?
                        GetChildren(screen.Id, screens) : null,
            }).ToList();

            return respnse;
        }
    }
}

