using Dawem.Data;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Dtos.Core.JustificationsTypes;

namespace Dawem.Contract.Repository.Core
{
    public interface IJustificationsTypeRepository : IGenericRepository<JustificationsType>
    {
        IQueryable<JustificationsType> GetAsQueryable(GetJustificationsTypeCriteria criteria);
    }
}
