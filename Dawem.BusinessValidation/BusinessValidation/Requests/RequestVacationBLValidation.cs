using Dawem.Contract.BusinessValidation.Requests;
using Dawem.Contract.Repository.Manager;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Requests.Vacations;
using Dawem.Models.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.Validation.BusinessValidation.Requests
{

    public class RequestVacationBLValidation : IRequestVacationBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        public RequestVacationBLValidation(IRepositoryManager _repositoryManager, RequestInfo _requestInfo)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
        }
        public async Task<int?> CreateValidation(CreateRequestVacationDTO model)
        {

            var CheckIfVacationEmployeesHasAnotherRequestTask = await repositoryManager
              .RequestTaskRepository.Get(c => !c.Request.IsDeleted &&
              (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
              c.Request.CompanyId == requestInfo.CompanyId &&
              (model.EmployeeId == null || model.EmployeeId == c.Request.EmployeeId) &&
              (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
              model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
              model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
              .Select(t => t.Request.Employee.Name)
              .Distinct()
              .Take(5)
              .ToListAsync();

            if (CheckIfVacationEmployeesHasAnotherRequestTask != null && CheckIfVacationEmployeesHasAnotherRequestTask.Count > 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeHasAnotherOverlappedTaskWithSelectedPeriod);
            }


            var CheckIfVacationEmployeesHasAnotherRequestVacation = await repositoryManager
                .RequestVacationRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                (model.EmployeeId == null || model.EmployeeId == c.Request.EmployeeId) &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfVacationEmployeesHasAnotherRequestVacation != null && CheckIfVacationEmployeesHasAnotherRequestVacation.Count > 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeHasAnotherOverlappedVacationWithSelectedPeriod);
            }

            var CheckIfVacationEmployeesHasAnotherRequestAssignment = await repositoryManager
                .RequestAssignmentRepository.Get(c => !c.Request.IsDeleted &&
                (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                c.Request.CompanyId == requestInfo.CompanyId &&
                (model.EmployeeId == null || model.EmployeeId == c.Request.EmployeeId) &&
                (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                .Select(t => t.Request.Employee.Name)
                .Distinct()
                .Take(5)
                .ToListAsync();

            if (CheckIfVacationEmployeesHasAnotherRequestAssignment != null && CheckIfVacationEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeHasAnotherOverlappedAssignmentWithSelectedPeriod);
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

            #region Validate And Set Balance

            var getVacationsType = await repositoryManager.VacationsTypeRepository.GetEntityByConditionAsync(c => !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId && c.Id == model.VacationTypeId) ?? 
                throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            var type = getVacationsType.DefaultType;

            var currentYear = DateTime.UtcNow.Year; /*65465654654*/

            var checkTypeBalance = await repositoryManager.VacationBalanceRepository.GetEntityByConditionAsync(c => !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId && c.EmployeeId == getCurrentEmployeeId
                && c.DefaultVacationType == type && c.Year == currentYear) ?? 
                throw new BusinessValidationException(LeillaKeys.SorryThereIsNoVacationBalanceOfSelectedVacationTypeForEmployee);

            var requiredDays = (model.DateTo - model.DateFrom).Days + 1;
            if (requiredDays > checkTypeBalance.RemainingBalance)
            {
                throw new BusinessValidationException(null,
                    LeillaKeys.SorryThereIsNoSufficientBalanceForSelectedTypeForEmployee +
                    LeillaKeys.Space +
                    LeillaKeys.CurrentBalanceForEmployee +
                    checkTypeBalance.RemainingBalance +
                    LeillaKeys.LeftBracket + TranslationHelper.GetTranslation(checkTypeBalance.DefaultVacationType.ToString(), requestInfo.Lang) +
                    LeillaKeys.RightBracket);
            }

            model.BalanceBeforeRequest = checkTypeBalance.RemainingBalance;
            model.BalanceAfterRequest = checkTypeBalance.RemainingBalance - requiredDays;

            #endregion

            return getCurrentEmployeeId;
        }
        public async Task<int?> UpdateValidation(UpdateRequestVacationDTO model)
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



            var CheckIfVacationEmployeesHasAnotherRequestVacation = await repositoryManager
          .RequestVacationRepository.Get(c =>
         !c.Request.IsDeleted &&
         (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
         c.Request.CompanyId == requestInfo.CompanyId &&
         c.Request.Id != model.Id && // Exclude the edited request
         (model.EmployeeId == null || model.EmployeeId == c.Request.EmployeeId) &&
         (
             (model.DateFrom.Date <= c.DateTo.Date && model.DateTo.Date >= c.Request.Date.Date) ||
             (model.DateTo.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date) ||
             (model.DateFrom.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date)))
            .Select(t => t.Request.Employee.Name)
            .Distinct()
            .Take(5)
            .ToListAsync();

            if (CheckIfVacationEmployeesHasAnotherRequestVacation != null && CheckIfVacationEmployeesHasAnotherRequestVacation.Count > 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeHasAnotherOverlappedVacationWithSelectedPeriod);
            }

            var CheckIfVacationEmployeesHasAnotherRequestAssignment = await repositoryManager
                           .RequestAssignmentRepository.Get(c => !c.Request.IsDeleted &&
                           (c.Request.Status == RequestStatus.Pending || c.Request.Status == RequestStatus.Accepted) &&
                           c.Request.CompanyId == requestInfo.CompanyId &&
                           (model.EmployeeId == null || model.EmployeeId == c.Request.EmployeeId) &&
                           (model.DateFrom.Date >= c.Request.Date.Date && model.DateFrom.Date <= c.DateTo.Date ||
                           model.DateTo.Date >= c.Request.Date.Date && model.DateTo.Date <= c.DateTo.Date ||
                           model.DateFrom.Date <= c.Request.Date.Date && model.DateTo.Date >= c.DateTo.Date))
                           .Select(t => t.Request.Employee.Name)
                           .Distinct()
                           .Take(5)
                           .ToListAsync();

            if (CheckIfVacationEmployeesHasAnotherRequestAssignment != null && CheckIfVacationEmployeesHasAnotherRequestAssignment.Count > 0)
            {
                throw new BusinessValidationException(AmgadKeys.SorryThisEmployeeHasAnotherOverlappedAssignmentWithSelectedPeriod);
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


            #region Validate And Set Balance

            var getVacationsType = await repositoryManager.VacationsTypeRepository.GetEntityByConditionAsync(c => !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId && c.Id == model.VacationTypeId) ?? throw new BusinessValidationException(LeillaKeys.SorryVacationTypeNotFound);

            var type = getVacationsType.DefaultType;

            var currentYear = DateTime.UtcNow.Year;

            var checkTypeBalance = await repositoryManager.VacationBalanceRepository.GetEntityByConditionAsync(c => !c.IsDeleted &&
                c.CompanyId == requestInfo.CompanyId && c.EmployeeId == getCurrentEmployeeId
                && c.DefaultVacationType == type && c.Year == currentYear) ?? throw new BusinessValidationException(LeillaKeys.SorryThereIsNoVacationBalanceOfSelectedVacationTypeForEmployee);

            var requiredDays = (model.DateTo - model.DateFrom).Days + 1;
            if (requiredDays > checkTypeBalance.RemainingBalance)
            {
                throw new BusinessValidationException(null,
                    LeillaKeys.SorryThereIsNoSufficientBalanceForSelectedTypeForEmployee +
                    LeillaKeys.Space +
                    LeillaKeys.CurrentBalanceForEmployee +
                    checkTypeBalance.RemainingBalance +
                    LeillaKeys.LeftBracket + TranslationHelper.GetTranslation(checkTypeBalance.DefaultVacationType.ToString(), requestInfo.Lang) +
                    LeillaKeys.RightBracket);
            }

            model.BalanceBeforeRequest = checkTypeBalance.RemainingBalance;
            model.BalanceAfterRequest = checkTypeBalance.RemainingBalance - requiredDays;

            #endregion

            return getCurrentEmployeeId;
        }
    }
}
