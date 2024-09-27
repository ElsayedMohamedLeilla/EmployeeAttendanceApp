using Dawem.Contract.Repository.MenuItems;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.MenuItems
{
    public class MenuItemNameTranslationRepository : GenericRepository<MenuItemNameTranslation>, IMenuItemNameTranslationRepository
    {
        private readonly RequestInfo _requestInfo;
        public MenuItemNameTranslationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
    }
}
