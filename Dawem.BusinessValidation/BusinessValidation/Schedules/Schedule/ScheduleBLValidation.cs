using Dawem.Contract.BusinessValidation.Schedules.Schedules;
using Dawem.Contract.Repository.Manager;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Schedules.Schedules;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Schedules.Schedule
{

    public class ScheduleBLValidation : IScheduleBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public ScheduleBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateScheduleModel model)
        {
            var checkWeekAttendanceDuplicate = await repositoryManager
                .ScheduleRepository.Get(c => c.CompanyId == requestInfo.CompanyId && c.Name == model.Name).AnyAsync();
            if (checkWeekAttendanceDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryScheduleNameIsDuplicated);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateScheduleModel model)
        {
            var checkWeekAttendanceDuplicate = await repositoryManager
                .ScheduleRepository.Get(c => c.CompanyId == requestInfo.CompanyId &&
                c.Name == model.Name && c.Id != model.Id).AnyAsync();
            if (checkWeekAttendanceDuplicate)
            {
                throw new BusinessValidationException(LeillaKeys.SorryScheduleNameIsDuplicated);
            }

            return true;
        }
    }
}
