using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.WeekAttendances
{

    public class WeekAttendanceBLValidation : IWeekAttendanceBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public WeekAttendanceBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateWeekAttendanceModel model)
        {
            var checkWeekAttendanceDuplicate = await repositoryManager
                .WeekAttendanceRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkWeekAttendanceDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryWeekAttendanceNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateWeekAttendanceModel model)
        {
            var checkWeekAttendanceDuplicate = await repositoryManager
                .WeekAttendanceRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkWeekAttendanceDuplicate)
            {
                throw new BusinessValidationException(DawemKeys.SorryWeekAttendanceNameIsDuplicated);
            }

            return true;
        }
    }
}
