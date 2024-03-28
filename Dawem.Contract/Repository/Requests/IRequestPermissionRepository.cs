using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Requests.Permissions;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestPermissionRepository : IGenericRepository<RequestPermission>
    {
        IQueryable<RequestPermission> GetAsQueryable(GetRequestPermissionsCriteria criteria);
        IQueryable<RequestPermission> EmployeeGetAsQueryable(EmployeeGetRequestPermissionsCriteria criteria);
    }
}
