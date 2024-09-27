using Dawem.Contract.Repository.MenuItems;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.MenuItems
{
    public class MenuItemActionRepository : GenericRepository<MenuItemAction>, IMenuItemActionRepository
    {
        private readonly RequestInfo _requestInfo;
        public MenuItemActionRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
    }
}
