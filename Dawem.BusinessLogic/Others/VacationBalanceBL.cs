using AutoMapper;
using Dawem.Contract.BusinessLogic.Schedules.VacationBalances;
using Dawem.Contract.BusinessValidation.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Requests;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Others.VacationBalances;
using Dawem.Models.Dtos.Schedules.SchedulePlans;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Requests.Vacations;
using Dawem.Models.Response.Schedules.SchedulePlans;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.VacationBalances.VacationBalances
{
    public class VacationBalanceBL : IVacationBalanceBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IVacationBalanceBLValidation vacationBalanceBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public VacationBalanceBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IVacationBalanceBLValidation _vacationBalanceBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            vacationBalanceBLValidation = _vacationBalanceBLValidation;
            mapper = _mapper;
        }
        public async Task<bool> Create(CreateVacationBalanceModel model)
        {
            #region Business Validation

            await vacationBalanceBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert Vacation Balance

            var employeesIds = await GetEmployeesIds(model);

            var getAllVacationBalance = await repositoryManager.VacationBalanceRepository
                .GetWithTracking(vacationBalance => employeesIds.Contains(vacationBalance.EmployeeId) &&
                vacationBalance.Year == model.Year &&
                vacationBalance.VacationType == model.VacationType).ToListAsync();

            foreach (var employeeId in employeesIds)
            {
                var checkForVacationBalance = getAllVacationBalance
                    .FirstOrDefault(vacationBalance => vacationBalance.EmployeeId == employeeId);

                if (checkForVacationBalance != null)
                {
                    checkForVacationBalance.Balance = model.Balance;
                    checkForVacationBalance.RemainingBalance = model.Balance;
                    checkForVacationBalance.ExpirationDate = new DateTime(model.Year, 12, 31);
                    checkForVacationBalance.ModifiedDate = DateTime.UtcNow;
                    checkForVacationBalance.ModifyUserId = requestInfo.UserId;
                }
                else
                {
                    #region Set Vacation Balance code

                    var getNextCode = await repositoryManager.VacationBalanceRepository
                        .Get(vacationBalance => vacationBalance.CompanyId == requestInfo.CompanyId)
                        .Select(vacationBalance => vacationBalance.Code)
                        .DefaultIfEmpty()
                        .MaxAsync() + 1;

                    #endregion

                    var vacationBalance = mapper.Map<VacationBalance>(model);
                    vacationBalance.CompanyId = requestInfo.CompanyId;
                    vacationBalance.AddUserId = requestInfo.UserId;
                    vacationBalance.EmployeeId = employeeId;
                    vacationBalance.Code = getNextCode;

                    repositoryManager.VacationBalanceRepository.Insert(vacationBalance);
                }
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion

        }
        private async Task<List<int>> GetEmployeesIds(CreateVacationBalanceModel model)
        {
            var ids = new List<int>();
            switch (model.ForType)
            {
                case Enums.Generals.ForType.Employees:
                    ids.Add(model.EmployeeId.Value);
                    break;
                case Enums.Generals.ForType.Groups:
                    ids = await repositoryManager.GroupEmployeeRepository.Get(g => !g.IsDeleted && g.GroupId == model.GroupId).Select(g => g.EmployeeId).ToListAsync();
                    break;
                case Enums.Generals.ForType.Departments:
                    ids = await repositoryManager.EmployeeRepository.Get(g => !g.IsDeleted && g.DepartmentId == model.DepartmentId).Select(g => g.Id).ToListAsync();
                    break;
                default:
                    break;
            }
            return ids;
        }
        public async Task<bool> Update(UpdateVacationBalanceModel model)
        {
            #region Business Validation

            await vacationBalanceBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update VacationBalance

            var getVacationBalance = await repositoryManager.VacationBalanceRepository
                .GetEntityByConditionWithTrackingAsync(vacationBalance => !vacationBalance.IsDeleted
            && vacationBalance.Id == model.Id);

            getVacationBalance.EmployeeId = model.EmployeeId;
            getVacationBalance.VacationType = model.VacationType;
            getVacationBalance.Year = model.Year;
            getVacationBalance.Balance = model.Balance;
            getVacationBalance.RemainingBalance = model.Balance;
            getVacationBalance.Notes = model.Notes;
            getVacationBalance.IsActive = model.IsActive;
            getVacationBalance.ModifiedDate = DateTime.Now;
            getVacationBalance.ModifyUserId = requestInfo.UserId;

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetVacationBalancesResponse> Get(GetVacationBalancesCriteria criteria)
        {
            var vacationBalanceRepository = repositoryManager.VacationBalanceRepository;
            var query = vacationBalanceRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = vacationBalanceRepository.OrderBy(query, nameof(VacationBalance.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var vacationBalancesList = await queryPaged.Select(vacationBalance => new GetVacationBalancesResponseModel
            {
                Id = vacationBalance.Id,
                Code = vacationBalance.Code,
                EmployeeName = vacationBalance.Employee.Name,
                VacationTypeName = TranslationHelper.GetTranslation(vacationBalance.VacationType.ToString(), requestInfo.Lang),
                Balance = vacationBalance.Balance,
                RemainingBalance = vacationBalance.RemainingBalance,
                Year = vacationBalance.Year,
                IsActive = vacationBalance.IsActive
            }).ToListAsync();

            return new GetVacationBalancesResponse
            {
                VacationBalances = vacationBalancesList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetVacationBalanceInfoResponseModel> GetInfo(int vacationBalanceId)
        {
            var vacationBalance = await repositoryManager.VacationBalanceRepository.Get(vacationBalance => vacationBalance.Id == vacationBalanceId && !vacationBalance.IsDeleted)
                .Select(vacationBalance => new GetVacationBalanceInfoResponseModel
                {
                    Code = vacationBalance.Code,
                    EmployeeName = vacationBalance.Employee.Name,
                    VacationTypeName = TranslationHelper.GetTranslation(vacationBalance.VacationType.ToString(), requestInfo.Lang),
                    VacationType = vacationBalance.VacationType,
                    Balance = vacationBalance.Balance,
                    RemainingBalance = vacationBalance.RemainingBalance,
                    Year = vacationBalance.Year,
                    IsActive = vacationBalance.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationBalanceNotFound);

            return vacationBalance;
        }
        public async Task<GetVacationBalanceByIdResponseModel> GetById(int vacationBalanceId)
        {
            var vacationBalance = await repositoryManager.VacationBalanceRepository
                .Get(vacationBalance => vacationBalance.Id == vacationBalanceId && !vacationBalance.IsDeleted)
                .Select(vacationBalance => new GetVacationBalanceByIdResponseModel
                {
                    Id = vacationBalance.Id,
                    Code = vacationBalance.Code,
                    EmployeeId = vacationBalance.EmployeeId,
                    VacationType = vacationBalance.VacationType,
                    Balance = vacationBalance.Balance,
                    Year = vacationBalance.Year,
                    Notes = vacationBalance.Notes,
                    IsActive = vacationBalance.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorryVacationBalanceNotFound);

            return vacationBalance;

        }
        public async Task<bool> Delete(int vacationBalanceId)
        {
            var vacationBalance = await repositoryManager.VacationBalanceRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == vacationBalanceId) ??
                throw new BusinessValidationException(LeillaKeys.SorryVacationBalanceNotFound);

            vacationBalance.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<GetVacationBalancesInformationsResponseDTO> GetVacationBalancesInformations()
        {
            var vacationBalanceRepository = repositoryManager.VacationBalanceRepository;
            var query = vacationBalanceRepository.Get(vacationBalance => vacationBalance.CompanyId == requestInfo.CompanyId);

            #region Handle Response

            return new GetVacationBalancesInformationsResponseDTO
            {
                TotalCount = await query.Where(vacationBalance => !vacationBalance.IsDeleted).CountAsync(),
                ActiveCount = await query.Where(vacationBalance => !vacationBalance.IsDeleted && vacationBalance.IsActive).CountAsync(),
                NotActiveCount = await query.Where(vacationBalance => !vacationBalance.IsDeleted && !vacationBalance.IsActive).CountAsync(),
                DeletedCount = await query.Where(vacationBalance => vacationBalance.IsDeleted).CountAsync()
            };

            #endregion
        }
    }
}