using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;

namespace Dawem.Contract.Repository.Core
{
    public interface IResponsibilityRepository : IGenericRepository<Responsibility>
    {
        IQueryable<Responsibility> GetAsQueryable(GetResponsibilitiesCriteria criteria);
    }
}
