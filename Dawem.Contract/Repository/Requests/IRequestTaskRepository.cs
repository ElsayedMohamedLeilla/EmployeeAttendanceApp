using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Requests.Tasks;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestTaskRepository : IGenericRepository<RequestTask>
    {
        IQueryable<RequestTask> GetAsQueryable(GetRequestTasksCriteria criteria);
    }
}
