using Dawem.Contract.Repository.Attendances;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendances;
using Dawem.Models.Context;
using Dawem.Models.Generic;

namespace Dawem.Repository.Attendances
{
    public class EmployeeAttendanceCheckRepository : GenericRepository<EmployeeAttendanceCheck>, IEmployeeAttendanceCheckRepository
    {
        private readonly RequestInfo requestInfo;
        public EmployeeAttendanceCheckRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
    }
}
