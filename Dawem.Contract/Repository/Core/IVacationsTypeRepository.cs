using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IVacationsTypeRepository : IGenericRepository<VacationType>
    {
        IQueryable<VacationType> GetAsQueryable(GetVacationsTypesCriteria criteria);
    }
}
