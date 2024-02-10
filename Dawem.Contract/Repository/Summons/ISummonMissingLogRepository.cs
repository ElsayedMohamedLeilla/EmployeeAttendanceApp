using Dawem.Data;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Dtos.Summons.Summons;

namespace Dawem.Contract.Repository.Summons
{
    public interface ISummonMissingLogRepository : IGenericRepository<SummonMissingLog>
    {
        IQueryable<SummonMissingLog> GetAsQueryable(GetSummonMissingLogsCriteria criteria);
    }
}
