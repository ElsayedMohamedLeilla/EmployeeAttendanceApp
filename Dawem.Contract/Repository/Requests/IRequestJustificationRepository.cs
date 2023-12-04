using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Requests.Justifications;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestJustificationRepository : IGenericRepository<RequestJustification>
    {
        IQueryable<RequestJustification> GetAsQueryable(GetRequestJustificationCriteria criteria);

    }
}
