using Dawem.Data;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Requests.Vacations;

namespace Dawem.Contract.Repository.Requests
{
    public interface IRequestVacationRepository : IGenericRepository<RequestVacation>
    {
        IQueryable<RequestVacation> GetAsQueryable(GetRequestVacationCriteria criteria);

    }
}
