using Dawem.Data;
using Dawem.Domain.Entities.Ohters;

namespace Dawem.Repository.Others.Conract
{
    public interface IActionLogRepository : IGenericRepository<ActionLog>
    {
        //IQueryable<ActionLog> GetAsQueryable(GetActionLogsCriteria criteria, string includeProperties = DawemKeys.EmptyString);
    }
}
