using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Requests.Justifications;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestOvertimeRepository : IGenericRepository<RequestOvertime>
    {
        IQueryable<RequestOvertime> GetAsQueryable(GetRequestOvertimeCriteria criteria);
        IQueryable<RequestOvertime> EmployeeGetAsQueryable(EmployeeGetRequestOvertimeCriteria criteria);
    }
}
