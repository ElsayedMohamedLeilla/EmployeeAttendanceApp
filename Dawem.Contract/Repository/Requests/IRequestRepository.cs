using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Employees.Department;
using Dawem.Models.Dtos.Requests;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestRepository : IGenericRepository<Request>
    {
        IQueryable<Request> GetAsQueryable(GetRequestsCriteria criteria);
        IQueryable<Request> GetForStatusAsQueryable(GetStatusBaseModel model);
        IQueryable<Request> EmployeeGetAsQueryable(EmployeeGetRequestsCriteria criteria);
    }
}
