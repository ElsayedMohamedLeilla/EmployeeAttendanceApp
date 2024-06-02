using Dawem.Data;
using Dawem.Domain.Entities.Others;
using Dawem.Models.DTOs.Dawem.Screens.Screens;

namespace Dawem.Contract.Repository.Settings
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        IQueryable<MenuItem> GetAsQueryable(GetScreensCriteria criteria);
    }
}
