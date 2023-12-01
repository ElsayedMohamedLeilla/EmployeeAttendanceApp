using Dawem.Contract.BusinessValidation.Employees;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.JobTitle;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Employees
{

    public class RequestTaskBLValidation : IRequestTaskBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public RequestTaskBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<bool> CreateValidation(CreateRequestTaskModelDTO model)
        {
            var getEmployeesThatOverlaped = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.EmployeeId) &&
                ((model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date) ||
                (model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date) ||
                (model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date)))
                .Select(t => t.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (getEmployeesThatOverlaped != null && getEmployeesThatOverlaped.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasAnotherRequestTaskThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, getEmployeesThatOverlaped) + LeillaKeys.RightBracket);
            }

            var CheckIfTaskEmployeesHasAnotherRequestVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.Request.EmployeeId) &&
                ((model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date) ||
                (model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date) ||
                (model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date)))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfTaskEmployeesHasAnotherRequestVacation != null && CheckIfTaskEmployeesHasAnotherRequestVacation.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, CheckIfTaskEmployeesHasAnotherRequestVacation) + LeillaKeys.RightBracket);
            }

            var CheckIfTaskEmployeesHasAnotherRequestAssignment = await repositoryManager
                .RequestAssignmentRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.Request.EmployeeId) &&
                ((model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date) ||
                (model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date) ||
                (model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date)))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfTaskEmployeesHasAnotherRequestAssignment != null && CheckIfTaskEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, CheckIfTaskEmployeesHasAnotherRequestAssignment) + LeillaKeys.RightBracket);
            }


            return true;
        }
        public async Task<bool> UpdateValidation(UpdateRequestTaskModelDTO model)
        {

            var getEmployeesThatOverlaped = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                c.RequestTask.Request.Id != model.Id &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.EmployeeId) &&
                ((model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date) ||
                (model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date) ||
                (model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date)))
                .Select(t => t.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (getEmployeesThatOverlaped != null && getEmployeesThatOverlaped.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasAnotherRequestTaskThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, getEmployeesThatOverlaped) + LeillaKeys.RightBracket);
            }

            var CheckIfTaskEmployeesHasAnotherRequestVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.Request.EmployeeId) &&
                ((model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date) ||
                (model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date) ||
                (model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date)))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfTaskEmployeesHasAnotherRequestVacation != null && CheckIfTaskEmployeesHasAnotherRequestVacation.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, CheckIfTaskEmployeesHasAnotherRequestVacation) + LeillaKeys.RightBracket);
            }

            var CheckIfTaskEmployeesHasAnotherRequestAssignment = await repositoryManager
                .RequestAssignmentRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.Request.EmployeeId) &&
                ((model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date) ||
                (model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date) ||
                (model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date)))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfTaskEmployeesHasAnotherRequestAssignment != null && CheckIfTaskEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, CheckIfTaskEmployeesHasAnotherRequestAssignment) + LeillaKeys.RightBracket);
            }

            return true;
        }
    }
}
