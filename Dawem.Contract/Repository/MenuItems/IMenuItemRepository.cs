using Dawem.Data;
using Dawem.Domain.Entities.Others;
using Dawem.Models.DTOs.Dawem.Screens.Screens;

namespace Dawem.Contract.Repository.MenuItems
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        IQueryable<MenuItem> GetAsQueryable(GetScreensCriteria criteria);
        IQueryable<MenuItem> GetAsQueryableForGetAllScreens(GetScreensCriteria criteria);
        IQueryable<MenuItem> GetAsQueryableForMenu(GetScreensCriteria criteria);
    }
}
