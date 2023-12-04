using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Requests.Assignments;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestAssignmentRepository : IGenericRepository<RequestAssignment>
    {
        IQueryable<RequestAssignment> GetAsQueryable(GetRequestAssignmentsCriteria criteria);
    }
}
