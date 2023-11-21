using Dawem.Contract.BusinessValidation.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Schedules.SchedulePlan
{

    public class SchedulePlanPlanBLValidation : ISchedulePlanBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public SchedulePlanPlanBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateSchedulePlanModel model)
        {
            var checkScheduleDuplicate = await repositoryManager
                .SchedulePlanRepository.Get(c => c.CompanyId == requestInfo.CompanyId
                && c.DateFrom == model.DateFrom
                && c.ScheduleId == model.ScheduleId
                && (c.SchedulePlanEmployee != null && c.SchedulePlanEmployee.EmployeeId == model.EmployeeId ||
                c.SchedulePlanGroup != null && c.SchedulePlanGroup.GroupId == model.GroupId ||
                c.SchedulePlanDepartment != null && c.SchedulePlanDepartment.DepartmentId == model.DepartmentId)).AnyAsync();

            if (checkScheduleDuplicate)
            {
                HandleDuplicateThrow(model.SchedulePlanType);
            }

            return true;
        }
        public async Task<bool> UpdateValidation(UpdateSchedulePlanModel model)
        {
            var checkScheduleDuplicate = await repositoryManager
                .SchedulePlanRepository.Get(c =>
                c.CompanyId == requestInfo.CompanyId
                && c.Id != model.Id
                && c.DateFrom == model.DateFrom
                && c.ScheduleId == model.ScheduleId
                && (c.SchedulePlanEmployee != null && c.SchedulePlanEmployee.EmployeeId == model.EmployeeId ||
                c.SchedulePlanGroup != null && c.SchedulePlanGroup.GroupId == model.GroupId ||
                c.SchedulePlanDepartment != null && c.SchedulePlanDepartment.DepartmentId == model.DepartmentId)).AnyAsync();
            if (checkScheduleDuplicate)
            {
                HandleDuplicateThrow(model.SchedulePlanType);
            }

            return true;
        }
        private void HandleDuplicateThrow(SchedulePlanType schedulePlanType)
        {
            switch (schedulePlanType)
            {
                case SchedulePlanType.Employees:
                    throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanIsDuplicatedWithSameDateAndScheduleAndEmployee);
                case SchedulePlanType.Groups:
                    throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanIsDuplicatedWithSameDateAndScheduleAndGroup);
                case SchedulePlanType.Departments:
                    throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanIsDuplicatedWithSameDateAndScheduleAndDepartment);
                default:
                    break;
            }
        }
    }
}
