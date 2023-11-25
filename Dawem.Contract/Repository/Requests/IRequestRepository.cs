using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestRepository : IGenericRepository<Request>
    {
        IQueryable<Request> GetAsQueryable(GetRequestsCriteria criteria);
    }
}
