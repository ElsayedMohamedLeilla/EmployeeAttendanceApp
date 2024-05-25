using Dawem.Data;
using Dawem.Domain.Entities.Others;
using Dawem.Models.DTOs.Dawem.Screens.ScreenGroups;

namespace Dawem.Contract.Repository.Settings
{
    public interface IScreenGroupRepository : IGenericRepository<ScreenGroup>
    {
        IQueryable<ScreenGroup> GetAsQueryable(GetScreenGroupsCriteria criteria);
    }
}
