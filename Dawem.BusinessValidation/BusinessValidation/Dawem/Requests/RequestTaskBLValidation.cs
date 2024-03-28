using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Requests.Tasks;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Requests
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
        public async Task<int?> CreateValidation(CreateRequestTaskModelDTO model)
        {
            var getEmployeesThatOverlaped = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.EmployeeId) &&
                (model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date ||
                model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date ||
                model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date))
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
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
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
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfTaskEmployeesHasAnotherRequestAssignment != null && CheckIfTaskEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasAssignmentRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, CheckIfTaskEmployeesHasAnotherRequestAssignment) + LeillaKeys.RightBracket);
            }

            int? getCurrentEmployeeId = null;

            if (!model.ForEmployee)
            {
                getCurrentEmployeeId = await repositoryManager.UserRepository
               .Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Id == requestInfo.UserId && e.EmployeeId != null).AnyAsync() ?
                 await repositoryManager.UserRepository
               .Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Id == requestInfo.UserId && e.EmployeeId != null)
               .Select(e => e.EmployeeId)
               .FirstOrDefaultAsync() : null;

                if (getCurrentEmployeeId == null)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);
                }
            }
            else
            {
                getCurrentEmployeeId = model.EmployeeId;
            }

            #region Validate Request Type

            var checkRequestType = await repositoryManager.TaskTypeRepository

                .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.TaskTypeId)
                .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion


            return getCurrentEmployeeId;
        }
        public async Task<int?> UpdateValidation(UpdateRequestTaskModelDTO model)
        {
            var getRequest = await repositoryManager.RequestRepository.Get(r => !r.IsDeleted && r.Id == model.Id)
                .Select(r => new
                {
                    r.Status
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryCannotFindRequest);

            if (getRequest.Status == RequestStatus.Accepted)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestIsAcceptedEditNotAllowed);
            }
            else if (getRequest.Status == RequestStatus.Rejected)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestIsRejectedEditNotAllowed);
            }

            var getEmployeesThatOverlaped = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                c.RequestTask.Request.Id != model.Id &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                model.TaskEmployeeIds.Contains(c.EmployeeId) &&
                (model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date ||
                model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date ||
                model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date))
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
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
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
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
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

            int? getCurrentEmployeeId = null;

            if (!model.ForEmployee)
            {
                getCurrentEmployeeId = await repositoryManager.UserRepository
               .Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Id == requestInfo.UserId && e.EmployeeId != null).AnyAsync() ?
                 await repositoryManager.UserRepository
               .Get(e => !e.IsDeleted && e.CompanyId == requestInfo.CompanyId && e.Id == requestInfo.UserId && e.EmployeeId != null)
               .Select(e => e.EmployeeId)
               .FirstOrDefaultAsync() : null;

                if (getCurrentEmployeeId == null)
                {
                    throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);
                }
            }
            else
            {
                getCurrentEmployeeId = model.EmployeeId;
            }

            #region Validate Request Type

            var checkRequestType = await repositoryManager.TaskTypeRepository

                .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.TaskTypeId)
                .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion

            return getCurrentEmployeeId;
        }
        public async Task<bool> GetEmployeeTasksValidation(EmployeeGetRequestTasksCriteria model)
        {
            var getEmployeeId = requestInfo?.EmployeeId;
            if (getEmployeeId <= 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);
            }

            var checkIfHasAttendances = await repositoryManager.RequestTaskEmployeeRepository
                .Get(a => !a.RequestTask.Request.IsDeleted && a.EmployeeId == getEmployeeId
                && (a.RequestTask.Request.Date.Month == model.Month
                && a.RequestTask.Request.Date.Year == model.Year || a.RequestTask.DateTo.Month == model.Month
                && a.RequestTask.DateTo.Year == model.Year))
                .AnyAsync();

            if (!checkIfHasAttendances)
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoTasksRequestsInSelectedYearAndMonth);

            return true;
        }
    }
}
