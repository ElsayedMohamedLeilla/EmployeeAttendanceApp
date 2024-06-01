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
using Dawem.Models.Response.Dawem.Others;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

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
        public async Task<GetAllScreensWithAvailableActionsResponse> OldGetAllScreensWithAvailableActions()
        {
            return new GetAllScreensWithAvailableActionsResponse
            {
                ScreensTypes = null
            };

            //var screenRepository = repositoryManager.MenuItemRepository;

            //var criteria = new GetScreensCriteria
            //{
            //    IsActive = true,
            //};
            //var query = screenRepository.GetAsQueryable(criteria);

            //#region paging

            //int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            //int take = PagingHelper.Take(criteria.PageSize);

            //#region sorting
            //var queryOrdered = screenRepository.OrderBy(query, nameof(MenuItem.AuthenticationType), LeillaKeys.Asc);
            //#endregion

            //var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            //#endregion

            //#region Handle Response

            //var screensTypes = await queryPaged.GroupBy(s => s.AuthenticationType).Select(screenGroup => new GetAllScreensWithAvailableActionsResponseModel
            //{
            //    Type = screenGroup.Key,
            //    TypeName = TranslationHelper.GetTranslation(screenGroup.Key.ToString() + nameof(AuthenticationType), requestInfo.Lang),

            //    ScreensGroups = screenGroup.Where(sg => sg.ParentId > 0).OrderBy(s => s.Order).
            //    GroupBy(s => s.ParentId).Select(screenGroupGroup => new GetAllScreensWithAvailableActionsModel
            //    {
            //        ScreenGroupId = screenGroupGroup.Key,
            //        Order = screenGroupGroup.First().Order,
            //        Name = screenGroupGroup.First().Screen.ScreenGroupNameTranslations.
            //        First(p => p.Language.ISO2 == requestInfo.Lang).Name,
            //        Screens = screenGroupGroup.OrderBy(s => s.Order).Select(s => new ScreenWithAvailableActionsDTO
            //        {
            //            Id = s.Id,
            //            Order = s.Order,
            //            Name = s.MenuItemNameTranslations.Any(p => p.Language.ISO2 == requestInfo.Lang) ?
            //            s.MenuItemNameTranslations.First(p => p.Language.ISO2 == requestInfo.Lang).Name : null,
            //            AvailableActions = s.MenuItemActions.Any() ? s.MenuItemActions.Select(a => a.ActionCode).ToList() : null,
            //        }).ToList()

            //    }).ToList(),

            //    Screens = screenGroup.Where(sg => sg.ParentId == null).
            //    OrderBy(s => s.Order).Select(s => new ScreenWithAvailableActionsDTO
            //    {
            //        Id = s.Id,
            //        Order = s.Order,
            //        Name = s.MenuItemNameTranslations.Any(p => p.Language.ISO2 == requestInfo.Lang) ?
            //        s.MenuItemNameTranslations.First(p => p.Language.ISO2 == requestInfo.Lang).Name : null,
            //        AvailableActions = s.MenuItemActions.Any() ? s.MenuItemActions.Select(a => a.ActionCode).ToList() : null,
            //    }).ToList()
            //}).ToListAsync();

            //return new GetAllScreensWithAvailableActionsResponse
            //{
            //    ScreensTypes = screensTypes
            //};

            //#endregion

        }
        public async Task<GetAllScreensWithAvailableActionsResponse> GetAllScreensWithAvailableActions()
        {
            var screenRepository = repositoryManager.MenuItemRepository;

            var criteria = new GetScreensCriteria
            {
                IsActive = true,
            };

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


            var query = screenRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting
            var queryOrdered = screenRepository.OrderBy(query, nameof(MenuItem.AuthenticationType), LeillaKeys.Asc);
            #endregion

            var queryPaged = criteria.GetPagingEnabled() ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var screensTypes = await queryPaged.
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
                var screenTypeModel = new GetAllScreensWithAvailableActionsResponseModel
                {
                    Type = screenType.Type,
                    TypeName = screenType.TypeName
                };
                var masterScreens = screenType.Screens.Where(s => s.ParentId == null).OrderBy(s => s.Order);

                foreach (var screen in masterScreens)
                {
                    screenTypeModel.Screens.Add(new ScreenWithAvailableActionsDTO
                    {
                        Id = screen.Id,
                        Type = screen.GroupOrScreenType,
                        Name = screen.Name,
                        Icon = screen.Icon,
                        URL = screen.URL,
                        AvailableActions = screen.AvailableActions,
                        Children = HasChildren(screen.Id, screenType.Screens) ?
                        GetChildren(screen.Id, screenType.Screens) : null,
                    });
                }

                response.ScreensTypes.Add(screenTypeModel);

            }

            return response;

            #endregion

        }
        private bool HasChildren(int screenId, List<ScreenDTO> screens)
        {
            return screens.Any(s => s.ParentId == screenId);
        }
        private List<ScreenWithAvailableActionsDTO> GetChildren(int screenId, List<ScreenDTO> screens)
        {
            var children = screens.Where(s => s.ParentId == screenId).ToList();

            var respnse = children.Select(screen => new ScreenWithAvailableActionsDTO
            {
                Id = screen.Id,
                Type = screen.GroupOrScreenType,
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

