using Dawem.Data;
using Dawem.Domain.Entities.Ohters;
using Dawem.Translations;
using SmartBusinessERP.Models.Criteria.Others;

namespace Dawem.Contract.Repository.Others
{
    public interface IActionLogRepository : IGenericRepository<ActionLog>
    {
        IQueryable<ActionLog> GetAsQueryable(GetActionLogsCriteria criteria, string includeProperties = DawemKeys.EmptyString);
    }
}
