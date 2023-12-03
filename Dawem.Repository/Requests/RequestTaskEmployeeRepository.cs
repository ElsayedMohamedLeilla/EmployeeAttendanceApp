using Dawem.Contract.Repository.Requests;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Generic;

namespace Dawem.Repository.Requests
{
    public class RequestTaskEmployeeRepository : GenericRepository<RequestTaskEmployee>, IRequestTaskEmployeeRepository
    {
        public RequestTaskEmployeeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }
}
