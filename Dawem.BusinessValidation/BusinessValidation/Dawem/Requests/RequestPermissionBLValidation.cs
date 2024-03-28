using Dawem.Contract.BusinessValidation.Dawem.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Requests.Permissions;
using Dawem.Models.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Dawem.Requests
{

    public class RequestPermissionBLValidation : IRequestPermissionBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public RequestPermissionBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<int?> CreateValidation(CreateRequestPermissionModelDTO model)
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

            var CheckIfEmployeeeVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.EmployeeId == c.Request.EmployeeId &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .FirstOrDefaultAsync();

            if (CheckIfEmployeeeVacation != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakePermissionRequestEmployeeHasVacationRequestInTheSameDate);
            }


            #region Validate Request Type

            var checkRequestType = await repositoryManager.PermissionsTypeRepository
                .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.PermissionTypeId)
                .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion


            return getCurrentEmployeeId;
        }
        public async Task<int?> UpdateValidation(UpdateRequestPermissionModelDTO model)
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

            var CheckIfEmployeeeVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                c.Request.Id != model.Id &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                model.EmployeeId == c.Request.EmployeeId &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .FirstOrDefaultAsync();

            if (CheckIfEmployeeeVacation != null)
            {
                throw new BusinessValidationException(LeillaKeys.SorryCannotMakePermissionRequestEmployeeHasVacationRequestInTheSameDate);
            }

            #region Validate Request Type

            var checkRequestType = await repositoryManager.PermissionsTypeRepository
                .Get(p => !p.IsDeleted && p.IsActive && p.CompanyId == requestInfo.CompanyId && p.Id == model.PermissionTypeId)
                .AnyAsync();
            if (!checkRequestType)
            {
                throw new BusinessValidationException(LeillaKeys.SorryRequestTypeNotFound);
            }

            #endregion

            return getCurrentEmployeeId;
        }
    }
}
