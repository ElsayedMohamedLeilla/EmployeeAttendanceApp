using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Generic;

namespace Dawem.Repository.Employees
{
    public class RequestVacationRepository : GenericRepository<RequestVacation>, IRequestVacationRepository
    {
        public RequestVacationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
