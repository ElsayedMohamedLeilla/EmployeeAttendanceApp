using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Models.Requests.Justifications;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Requests
{

    public class RequestOvertimeBLValidation : IRequestOvertimeBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public RequestOvertimeBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<int?> CreateValidation(CreateRequestOvertimeDTO model)
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

            var checkIfEmployeeeOvertime = await repositoryManager
               .RequestOvertimeRepository.Get(c => !c.Request.IsDeleted &&
               (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
               c.Request.CompanyId == requestInfo.CompanyId &&
               getCurrentEmployeeId == c.Request.EmployeeId &&
               ((model.DateFrom > c.DateFrom && model.DateFrom < c.DateTo) ||
               (model.DateTo > c.DateFrom && model.DateTo < c.DateTo) ||
               (model.DateFrom == c.DateFrom && model.DateTo == c.DateTo)))
               .FirstOrDefaultAsync();

            if (checkIfEmployeeeOvertime != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakeOvertimeRequestEmployeeHasOvertimeRequestInTheSameTime);
            }

            var checkIfemployeeAttend = await repositoryManager.EmployeeAttendanceRepository.
                Get(a => a.CompanyId == requestInfo.CompanyId &&
                a.EmployeeId == getCurrentEmployeeId &&
                a.LocalDate.Date == model.OvertimeDate.Date).
                AnyAsync();
            if (!checkIfemployeeAttend)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeHasNoAttendanceAtTheOvertimeDate);
            }


            #region Validate Request Type

            var checkRequestType = await repositoryManager.OvertimeTypeRepository
                .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.OvertimeTypeId)
                .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion


            return getCurrentEmployeeId;
        }
        public async Task<int?> UpdateValidation(UpdateRequestOvertimeDTO model)
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

            var checkIfEmployeeeOvertime = await repositoryManager
               .RequestOvertimeRepository.Get(c => !c.Request.IsDeleted &&
               (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
               c.Request.CompanyId == requestInfo.CompanyId &&
               getCurrentEmployeeId == c.Request.EmployeeId &&
               ((model.DateFrom > c.DateFrom && model.DateFrom < c.DateTo) ||
               (model.DateTo > c.DateFrom && model.DateTo < c.DateTo) ||
               (model.DateFrom == c.DateFrom && model.DateTo == c.DateTo)))
               .FirstOrDefaultAsync();

            if (checkIfEmployeeeOvertime != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakeOvertimeRequestEmployeeHasOvertimeRequestInTheSameTime);
            }

            var checkIfemployeeAttend = await repositoryManager.EmployeeAttendanceRepository.
                Get(a => a.CompanyId == requestInfo.CompanyId &&
                a.EmployeeId == getCurrentEmployeeId &&
                a.LocalDate.Date == model.OvertimeDate.Date).
                AnyAsync();
            if (!checkIfemployeeAttend)
            {
                throw new BusinessValidationException(LeillaKeys.SorryEmployeeHasNoAttendanceAtTheOvertimeDate);
            }

            #region Validate Request Type

            var checkOvertimeType = await repositoryManager.OvertimeTypeRepository
                .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.OvertimeTypeId)
                .AnyAsync();
            if (!checkOvertimeType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion

            return getCurrentEmployeeId;
        }
    }
}
