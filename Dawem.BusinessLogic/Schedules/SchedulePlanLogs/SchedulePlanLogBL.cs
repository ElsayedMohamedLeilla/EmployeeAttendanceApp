using AutoMapper;
using Dawem.Contract.BusinessLogic.Schedules.SchedulePlanLogs;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Schedules;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Schedules.SchedulePlanLogs;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Schedules.SchedulePlanLogs
{
    public class SchedulePlanLogBL : ISchedulePlanLogBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public SchedulePlanLogBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            mapper = _mapper;
        }
        public async Task<GetSchedulePlanLogInfoResponseModel> GetInfo(int schedulePlanLogId)
        {
            var schedulePlanLog = await repositoryManager.SchedulePlanLogRepository
                .Get(schedulePlanLog => schedulePlanLog.Id == schedulePlanLogId &&
                !schedulePlanLog.IsDeleted)
                .Select(schedulePlanLog => new GetSchedulePlanLogInfoResponseModel
                {
                    Code = schedulePlanLog.Code,
                    SchedulePlanType = schedulePlanLog.SchedulePlan.SchedulePlanType,
                    SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlanLog.SchedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                    ScheduleName = schedulePlanLog.SchedulePlan.Schedule.Name,
                    EmployeeName = schedulePlanLog.SchedulePlan.SchedulePlanEmployee.Employee.Name,
                    GroupName = schedulePlanLog.SchedulePlan.SchedulePlanGroup.Group.Name,
                    DepartmentName = schedulePlanLog.SchedulePlan.SchedulePlanDepartment.Department.Name,
                    ApplyDate = schedulePlanLog.StartDate,
                    ScheduleDateFrom = schedulePlanLog.SchedulePlan.DateFrom,
                    Notes = schedulePlanLog.Notes,
                    EmployeesNumberAppliedOn = schedulePlanLog.SchedulePlanLogEmployees.Count,
                    EmployeesAppliedOn = schedulePlanLog.SchedulePlanLogEmployees
                    .OrderByDescending(e => e.Id)
                    .Take(5)
                    .Select(e => new GetSchedulePlanLogEmployeeInfoModel
                    {
                        EmployeeName = e.Employee.Name,
                        OldScheduleName = e.OldSchedule.Name,
                        NewScheduleName = e.NewSchedule.Name
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanLogNotFound);

            return schedulePlanLog;
        }
        public async Task<GetSchedulePlanLogsResponse> Get(GetSchedulePlanLogCriteria criteria)
        {
            var schedulePlanLogRepository = repositoryManager.SchedulePlanLogRepository;
            var query = schedulePlanLogRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = schedulePlanLogRepository.OrderBy(query, nameof(SchedulePlanLog.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlanLogsList = await queryPaged.Select(schedulePlanLog => new GetSchedulePlanLogsResponseModel
            {
                Id = schedulePlanLog.Id,
                ScheduleName = schedulePlanLog.SchedulePlan.Schedule.Name,
                SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlanLog.SchedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                ApplyDate = schedulePlanLog.StartDate,
                EmployeesNumberAppliedOn = schedulePlanLog.SchedulePlanLogEmployees.Count
            }).ToListAsync();

            return new GetSchedulePlanLogsResponse
            {
                SchedulePlanLogs = schedulePlanLogsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSchedulePlanLogEmployeesResponse> GetSchedulePlanLogEmployees(GetSchedulePlanLogEmployeesCriteria model)
        {
            #region Validation

            if (model.SchedulePlanLogId <= 0)
                throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanLogNotFound);

            #endregion

            var schedulePlanLogEmployeeRepository = repositoryManager.SchedulePlanLogEmployeeRepository;
            var query = schedulePlanLogEmployeeRepository
                .Get(e=> !e.IsDeleted && e.SchedulePlanLogId == model.SchedulePlanLogId);

            #region paging

            int skip = PagingHelper.Skip(model.PageNumber, model.PageSize);
            int take = PagingHelper.Take(model.PageSize);

            #region sorting

            var queryOrdered = schedulePlanLogEmployeeRepository.OrderBy(query, nameof(SchedulePlanLogEmployee.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = model.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlanLogsList = await queryPaged.Select(e => new GetSchedulePlanLogEmployeeInfoModel
            {
                EmployeeName = e.Employee.Name,
                OldScheduleName = e.OldSchedule.Name,
                NewScheduleName = e.NewSchedule.Name
            }).ToListAsync();

            return new GetSchedulePlanLogEmployeesResponse
            {
                Employees = schedulePlanLogsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
    }
}