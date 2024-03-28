using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Requests.Tasks;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestTaskRepository : IGenericRepository<RequestTask>
    {
        IQueryable<RequestTask> GetAsQueryable(GetRequestTasksCriteria criteria);
        IQueryable<RequestTask> EmployeeGetAsQueryable(Employee2GetRequestTasksCriteria criteria);
    }
}
