﻿using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Requests.Tasks;
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
            #region Request Employee

            int? getCurrentEmployeeId = null;

            if (!model.ForEmployee)
            {
                getCurrentEmployeeId = model.EmployeeId = await repositoryManager.UserRepository
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

            #endregion

            var getEmployeesThatOverlapedRequestTaskEmployees = await repositoryManager
                .RequestTaskEmployeeRepository.Get(c => !c.RequestTask.Request.IsDeleted &&
                (c.RequestTask.Request.Status == RequestStatus.Pending || c.RequestTask.Request.Status == RequestStatus.Accepted) &&
                c.RequestTask.Request.CompanyId == requestInfo.CompanyId &&
                (c.RequestTask.Request.EmployeeId == model.EmployeeId || model.TaskEmployeeIds.Contains(c.EmployeeId)) &&
                (model.DateFrom.Date >= c.RequestTask.Request.Date.Date && model.DateFrom.Date <= c.RequestTask.DateTo.Date ||
                model.DateTo.Date >= c.RequestTask.Request.Date.Date && model.DateTo.Date <= c.RequestTask.DateTo.Date ||
                model.DateFrom.Date <= c.RequestTask.Request.Date.Date && model.DateTo.Date >= c.RequestTask.DateTo.Date))
                .Select(t => t.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (getEmployeesThatOverlapedRequestTaskEmployees != null && getEmployeesThatOverlapedRequestTaskEmployees.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasAnotherRequestTaskThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, getEmployeesThatOverlapedRequestTaskEmployees) + LeillaKeys.RightBracket);
            }

            var getEmployeesThatOverlapedRequestTask = await repositoryManager
                .RequestTaskRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                c.Request.EmployeeId == model.EmployeeId &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (getEmployeesThatOverlapedRequestTask != null && getEmployeesThatOverlapedRequestTask.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasAnotherRequestTaskThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, getEmployeesThatOverlapedRequestTask) + LeillaKeys.RightBracket);
            }

            var checkIfTaskEmployeesHasAnotherRequestVacation = await repositoryManager
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

            if (checkIfTaskEmployeesHasAnotherRequestVacation != null && checkIfTaskEmployeesHasAnotherRequestVacation.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, checkIfTaskEmployeesHasAnotherRequestVacation) + LeillaKeys.RightBracket);
            }

            var checkIfTaskEmployeesHasAnotherRequestAssignment = await repositoryManager
                .RequestAssignmentRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                (c.Request.EmployeeId == model.EmployeeId || model.TaskEmployeeIds.Contains(c.Request.EmployeeId)) &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (checkIfTaskEmployeesHasAnotherRequestAssignment != null && checkIfTaskEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasAssignmentRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, checkIfTaskEmployeesHasAnotherRequestAssignment) + LeillaKeys.RightBracket);
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

            var checkIfTaskEmployeesHasAnotherRequestVacation = await repositoryManager
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

            if (checkIfTaskEmployeesHasAnotherRequestVacation != null && checkIfTaskEmployeesHasAnotherRequestVacation.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, checkIfTaskEmployeesHasAnotherRequestVacation) + LeillaKeys.RightBracket);
            }

            var checkIfTaskEmployeesHasAnotherRequestAssignment = await repositoryManager
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

            if (checkIfTaskEmployeesHasAnotherRequestAssignment != null && checkIfTaskEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(messageCode: null,
                    message: TranslationHelper
                    .GetTranslation(LeillaKeys.SorryYouChooseEmployeesThatHasVacationRequestThatOverlappedInDate, requestInfo.Lang)
                    + LeillaKeys.Space +
                    TranslationHelper.GetTranslation(LeillaKeys.EmployeesNames, requestInfo.Lang)
                    + LeillaKeys.LeftBracket + string.Join(LeillaKeys.CommaThenSpace, checkIfTaskEmployeesHasAnotherRequestAssignment) + LeillaKeys.RightBracket);
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

            var checkIfHasAttendances = await repositoryManager.RequestTaskRepository
                .Get(a => !a.Request.IsDeleted && (a.Request.EmployeeId == getEmployeeId ||
               a.TaskEmployees.Any(e => e.EmployeeId == getEmployeeId))
               && ((a.Request.Date.Month == model.Month
               && a.Request.Date.Year == model.Year) || (a.Request.RequestTask.DateTo.Month == model.Month
               && a.Request.RequestTask.DateTo.Year == model.Year)))
                .AnyAsync();

            if (!checkIfHasAttendances)
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoTasksRequestsInSelectedYearAndMonth);

            return true;
        }
    }
}
