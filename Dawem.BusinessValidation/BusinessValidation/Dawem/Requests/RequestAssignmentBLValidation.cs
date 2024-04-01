using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Requests.Assignments;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Requests
{

    public class RequestAssignmentBLValidation : IRequestAssignmentBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public RequestAssignmentBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<int?> CreateValidation(CreateRequestAssignmentModelDTO model)
        {
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

            var CheckIfEmployeeeHasVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.EmployeeId == c.Request.EmployeeId &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .FirstOrDefaultAsync();

            if (CheckIfEmployeeeHasVacation != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakeAssignmentRequestEmployeeHasVacationRequestInTheSameDate);
            }

            var CheckIfEmployeeeHasTask = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                model.EmployeeId == c.RequestTask.Request.EmployeeId &&
                (model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date ||
                model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date ||
                model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date))
                .FirstOrDefaultAsync();

            if (CheckIfEmployeeeHasVacation != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakeAssignmentRequestEmployeeHasTaskRequestInTheSameDate);
            }

            #region Validate Request Type

            var checkRequestType = await repositoryManager.AssignmentTypeRepository
               .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.AssignmentTypeId)
               .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion

            return getCurrentEmployeeId;
        }
        public async Task<int?> UpdateValidation(UpdateRequestAssignmentModelDTO model)
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

            var CheckIfEmployeeeHasVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                c.Request.Id != model.Id &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.EmployeeId == c.Request.EmployeeId &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .FirstOrDefaultAsync();

            if (CheckIfEmployeeeHasVacation != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakeAssignmentRequestEmployeeHasVacationRequestInTheSameDate);
            }

            var CheckIfEmployeeeHasTask = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                model.EmployeeId == c.RequestTask.Request.EmployeeId &&
                (model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date ||
                model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date ||
                model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date))
                .FirstOrDefaultAsync();

            if (CheckIfEmployeeeHasVacation != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakeAssignmentRequestEmployeeHasTaskRequestInTheSameDate);
            }

            #region Validate Request Type

            var checkRequestType = await repositoryManager.AssignmentTypeRepository
               .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.AssignmentTypeId)
               .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion

            return getCurrentEmployeeId;
        }
        public async Task<bool> GetEmployeeAssignmentsValidation(EmployeeGetRequestAssignmentsCriteria model)
        {
            var getEmployeeId = requestInfo?.EmployeeId;
            if (getEmployeeId <= 0)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCurrentUserNotEmployee);
            }

            var checkIfHasAttendances = await repositoryManager.RequestAssignmentRepository
                .Get(a => !a.Request.IsDeleted && a.Request.EmployeeId == getEmployeeId
                && a.Request.Date.Month == model.Month
                && a.Request.Date.Year == model.Year)
                .AnyAsync();

            if (!checkIfHasAttendances)
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoAssignmentsRequestsInSelectedYearAndMonth);

            return true;
        }
    }
}
