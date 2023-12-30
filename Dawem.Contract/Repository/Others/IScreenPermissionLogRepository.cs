using Dawem.Data;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;

namespace Dawem.Contract.Repository.Others
{
    public interface IScreenPermissionLogRepository : IGenericRepository<ScreenPermissionLog>
    {
        IQueryable<ScreenPermissionLog> GetAsQueryable(GetScreenPermissionLogsCriteria criteria, string includeProperties = LeillaKeys.EmptyString);
    }
}
