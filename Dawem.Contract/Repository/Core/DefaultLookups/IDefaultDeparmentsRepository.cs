using Dawem.Data;
using Dawem.Domain.Entities.Core.DefaultLookus;
using Dawem.Models.Criteria.DefaultLookups;

namespace Dawem.Contract.Repository.Core.DefaultLookups
{
    public interface IDefaultDepartmentsRepository : IGenericRepository<DefaultLookup>
    {
        IQueryable<DefaultLookup> GetAsQueryable(GetDefaultDepartmentsCriteria criteria);
    }
}
