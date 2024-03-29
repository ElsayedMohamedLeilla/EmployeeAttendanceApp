using Dawem.Data;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Dtos.Dawem.Summons.Summons;

namespace Dawem.Contract.Repository.Summons
{
    public interface ISummonLogRepository : IGenericRepository<SummonLog>
    {
        IQueryable<SummonLog> GetAsQueryable(GetSummonLogsCriteria criteria);
    }
}
