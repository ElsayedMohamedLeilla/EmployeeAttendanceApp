using Dawem.Contract.Repository.Attendances;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Attendances
{
    public class FingerprintDeviceLogRepository : GenericRepository<FingerprintDeviceLog>, IFingerprintDeviceLogRepository
    {
        private readonly RequestInfo requestInfo;
        public FingerprintDeviceLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo) : base(unitOfWork, _generalSetting)
        {
            requestInfo = _requestInfo;
        }
    }
}
