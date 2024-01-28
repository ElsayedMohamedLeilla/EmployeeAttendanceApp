using Dawem.Data;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Dtos.Summons.Sanctions;

namespace Dawem.Contract.Repository.Summons
{
    public interface ISanctionRepository : IGenericRepository<Sanction>
    {
        IQueryable<Sanction> GetAsQueryable(GetSanctionsCriteria criteria);
    }
}
