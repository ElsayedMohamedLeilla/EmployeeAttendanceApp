using Dawem.Data;
using Dawem.Domain.Entities.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        //IQueryable<Group> GetAsQueryable(GetGroupsCriteria criteria, string includeProperties = DawemKeys.EmptyString);
    }
}
