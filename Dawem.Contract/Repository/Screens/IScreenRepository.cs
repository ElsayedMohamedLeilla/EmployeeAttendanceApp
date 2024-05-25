using Dawem.Data;
using Dawem.Domain.Entities.Others;
using Dawem.Models.DTOs.Dawem.Screens;

namespace Dawem.Contract.Repository.Settings
{
    public interface IScreenRepository : IGenericRepository<Screen>
    {
        IQueryable<Screen> GetAsQueryable(GetScreensCriteria criteria);
    }
}
