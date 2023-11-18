using AutoMapper;
using Dawem.Contract.BusinessLogic.Attendances.SchedulePlanBackgroudJobLogs;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Attendances.Schedules;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Attendances.SchedulePlanBackgroundJobLogs
{
    public class SchedulePlanBackgroundJobLogBL : ISchedulePlanBackgroundJobLogBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public SchedulePlanBackgroundJobLogBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            mapper = _mapper;
        }
        public async Task<GetSchedulePlanBackgroundJobLogInfoResponseModel> GetInfo(int schedulePlanBackgroundJobLogId)
        {
            var schedulePlanBackgroundJobLog = await repositoryManager.SchedulePlanBackgroundJobLogRepository
                .Get(schedulePlanBackgroundJobLog => schedulePlanBackgroundJobLog.Id == schedulePlanBackgroundJobLogId &&
                !schedulePlanBackgroundJobLog.IsDeleted)
                .Select(schedulePlanBackgroundJobLog => new GetSchedulePlanBackgroundJobLogInfoResponseModel
                {
                    Code = schedulePlanBackgroundJobLog.Code,
                    SchedulePlanType = schedulePlanBackgroundJobLog.SchedulePlan.SchedulePlanType,
                    SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlanBackgroundJobLog.SchedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                    ScheduleName = schedulePlanBackgroundJobLog.SchedulePlan.Schedule.Name,
                    EmployeeName = schedulePlanBackgroundJobLog.SchedulePlan.SchedulePlanEmployee.Employee.Name,
                    GroupName = schedulePlanBackgroundJobLog.SchedulePlan.SchedulePlanGroup.Group.Name,
                    DepartmentName = schedulePlanBackgroundJobLog.SchedulePlan.SchedulePlanDepartment.Department.Name,
                    DateFrom = schedulePlanBackgroundJobLog.SchedulePlan.DateFrom,
                    Notes = schedulePlanBackgroundJobLog.Notes,
                    EmployeesNumberAppliedOn = schedulePlanBackgroundJobLog.SchedulePlanBackgroundJobLogEmployees.Count,
                    EmployeesAppliedOn = schedulePlanBackgroundJobLog.SchedulePlanBackgroundJobLogEmployees
                    .Select(e => new GetSchedulePlanBackgroundJobLogEmployeeInfoModel
                    {
                        EmployeeName = e.Employee.Name,
                        OldScheduleName = e.OldSchedule.Name,
                        NewScheduleName = e.NewSchedule.Name
                    }).ToList()
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanBackgroundJobLogNotFound);

            return schedulePlanBackgroundJobLog;
        }
        public async Task<GetSchedulePlanBackgroundJobLogsResponse> Get(GetSchedulePlanBackgroundJobLogsCriteria criteria)
        {
            var schedulePlanBackgroundJobLogRepository = repositoryManager.SchedulePlanBackgroundJobLogRepository;
            var query = schedulePlanBackgroundJobLogRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = schedulePlanBackgroundJobLogRepository.OrderBy(query, nameof(SchedulePlanBackgroundJobLog.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlanBackgroundJobLogsList = await queryPaged.Select(schedulePlanBackgroundJobLog => new GetSchedulePlanBackgroundJobLogsResponseModel
            {
                Id = schedulePlanBackgroundJobLog.Id,
                ScheduleName = schedulePlanBackgroundJobLog.SchedulePlan.Schedule.Name,
                SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlanBackgroundJobLog.SchedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                StartDate = schedulePlanBackgroundJobLog.StartDate,
                FinishDate = schedulePlanBackgroundJobLog.FinishDate,
                EmployeesNumberAppliedOn = schedulePlanBackgroundJobLog.SchedulePlanBackgroundJobLogEmployees.Count
            }).ToListAsync();

            return new GetSchedulePlanBackgroundJobLogsResponse
            {
                SchedulePlanBackgroundJobLogs = schedulePlanBackgroundJobLogsList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
    }
}