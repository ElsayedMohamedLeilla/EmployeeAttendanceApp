using AutoMapper;
using Dawem.Contract.BusinessLogic.Attendances.SchedulePlans;
using Dawem.Contract.BusinessValidation.Attendances.SchedulePlans;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Attendance;
using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Employees.Employees;
using Dawem.Models.Exceptions;
using Dawem.Models.Response.Attendances.Schedules;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.Attendances.SchedulePlans
{
    public class SchedulePlanBL : ISchedulePlanBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISchedulePlanBLValidation schedulePlanBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        public SchedulePlanBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           ISchedulePlanBLValidation _schedulePlanBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            schedulePlanBLValidation = _schedulePlanBLValidation;
            mapper = _mapper;
        }
        public async Task<int> Create(CreateSchedulePlanModel model)
        {
            #region Business Validation

            await schedulePlanBLValidation.CreateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Insert SchedulePlan

            #region Set SchedulePlan code

            var getNextCode = await repositoryManager.SchedulePlanRepository
                .Get(schedulePlan => schedulePlan.CompanyId == requestInfo.CompanyId)
                .Select(schedulePlan => schedulePlan.Code)
                .DefaultIfEmpty()
                .MaxAsync() + 1;

            #endregion

            var schedulePlan = mapper.Map<SchedulePlan>(model);
            schedulePlan.CompanyId = requestInfo.CompanyId;
            schedulePlan.AddUserId = requestInfo.UserId;
            schedulePlan.Code = getNextCode;
            repositoryManager.SchedulePlanRepository.Insert(schedulePlan);
            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return schedulePlan.Id;

            #endregion

        }
        public async Task<bool> Update(UpdateSchedulePlanModel model)
        {
            #region Business Validation

            await schedulePlanBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update SchedulePlan

            var getSchedulePlan = await repositoryManager.SchedulePlanRepository
                .GetWithTracking(schedulePlan => !schedulePlan.IsDeleted
            && schedulePlan.Id == model.Id)
                .Include(s => s.SchedulePlanEmployee)
                .Include(s => s.SchedulePlanGroup)
                .Include(s => s.SchedulePlanDepartment)
                .FirstOrDefaultAsync();


            getSchedulePlan.ScheduleId = model.ScheduleId;
            getSchedulePlan.SchedulePlanType = model.SchedulePlanType;
            getSchedulePlan.DateFrom = model.DateFrom;
            getSchedulePlan.Notes = model.Notes;
            getSchedulePlan.IsActive = model.IsActive;
            getSchedulePlan.ModifiedDate = DateTime.Now;
            getSchedulePlan.ModifyUserId = requestInfo.UserId;

            #region Handle Related

            #region Employee

            if (model.EmployeeId != null)
            {
                if (getSchedulePlan.SchedulePlanEmployee != null)
                {
                    getSchedulePlan.SchedulePlanEmployee.EmployeeId = model.EmployeeId ?? 0;
                }
                else
                {
                    getSchedulePlan.SchedulePlanEmployee = new SchedulePlanEmployee { EmployeeId = model.EmployeeId ?? 0 };
                }
            }
            else
            {
                getSchedulePlan.SchedulePlanEmployee = null;
            }

            #endregion

            #region Group

            if (model.GroupId != null)
            {
                if (getSchedulePlan.SchedulePlanGroup != null)
                {
                    getSchedulePlan.SchedulePlanGroup.GroupId = model.GroupId ?? 0;
                }
                else
                {
                    getSchedulePlan.SchedulePlanGroup = new SchedulePlanGroup { GroupId = model.GroupId ?? 0 };
                }
            }
            else
            {
                getSchedulePlan.SchedulePlanGroup = null;
            }

            #endregion

            #region Department

            if (model.DepartmentId != null)
            {
                if (getSchedulePlan.SchedulePlanDepartment != null)
                {
                    getSchedulePlan.SchedulePlanDepartment.DepartmentId = model.DepartmentId ?? 0;
                }
                else
                {
                    getSchedulePlan.SchedulePlanDepartment = new SchedulePlanDepartment { DepartmentId = model.DepartmentId ?? 0 };
                }
            }
            else
            {
                getSchedulePlan.SchedulePlanDepartment = null;
            }

            #endregion

            #endregion

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetSchedulePlansResponse> Get(GetSchedulePlansCriteria criteria)
        {
            var schedulePlanRepository = repositoryManager.SchedulePlanRepository;
            var query = schedulePlanRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = schedulePlanRepository.OrderBy(query, nameof(SchedulePlan.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlansList = await queryPaged.Select(schedulePlan => new GetSchedulePlansResponseModel
            {
                Id = schedulePlan.Id,
                Code = schedulePlan.Code,
                ScheduleName = schedulePlan.Schedule.Name,
                SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                DateFrom = schedulePlan.DateFrom,
                IsActive = schedulePlan.IsActive
            }).ToListAsync();

            return new GetSchedulePlansResponse
            {
                SchedulePlans = schedulePlansList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSchedulePlansForDropDownResponse> GetForDropDown(GetSchedulePlansCriteria criteria)
        {
            criteria.IsActive = true;
            var schedulePlanRepository = repositoryManager.SchedulePlanRepository;
            var query = schedulePlanRepository.GetAsQueryable(criteria);

            #region paging

            int skip = PagingHelper.Skip(criteria.PageNumber, criteria.PageSize);
            int take = PagingHelper.Take(criteria.PageSize);

            #region sorting

            var queryOrdered = schedulePlanRepository.OrderBy(query, nameof(SchedulePlan.Id), LeillaKeys.Desc);

            #endregion

            var queryPaged = criteria.PagingEnabled ? queryOrdered.Skip(skip).Take(take) : queryOrdered;

            #endregion

            #region Handle Response

            var schedulePlansList = await queryPaged.Select(schedulePlan => new GetSchedulePlansForDropDownResponseModel
            {
                Id = schedulePlan.Id,
                Name = schedulePlan.Schedule.Name
            }).ToListAsync();

            return new GetSchedulePlansForDropDownResponse
            {
                SchedulePlans = schedulePlansList,
                TotalCount = await query.CountAsync()
            };

            #endregion

        }
        public async Task<GetSchedulePlanInfoResponseModel> GetInfo(int schedulePlanId)
        {
            var schedulePlan = await repositoryManager.SchedulePlanRepository
                .Get(schedulePlan => schedulePlan.Id == schedulePlanId && !schedulePlan.IsDeleted)
                .Select(schedulePlan => new GetSchedulePlanInfoResponseModel
                {
                    Code = schedulePlan.Code,
                    SchedulePlanType = schedulePlan.SchedulePlanType,
                    SchedulePlanTypeName = TranslationHelper.GetTranslation(schedulePlan.SchedulePlanType.ToString(), requestInfo.Lang),
                    ScheduleName = schedulePlan.Schedule.Name,
                    EmployeeName = schedulePlan.SchedulePlanEmployee.Employee.Name,
                    GroupName = schedulePlan.SchedulePlanGroup.Group.Name,
                    DepartmentName = schedulePlan.SchedulePlanDepartment.Department.Name,
                    DateFrom = schedulePlan.DateFrom,
                    Notes = schedulePlan.Notes,
                    IsActive = schedulePlan.IsActive
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanNotFound);

            return schedulePlan;
        }
        public async Task<GetSchedulePlanByIdResponseModel> GetById(int schedulePlanId)
        {
            var schedulePlan = await repositoryManager.SchedulePlanRepository.Get(schedulePlan => schedulePlan.Id == schedulePlanId && !schedulePlan.IsDeleted)
                .Select(schedulePlan => new GetSchedulePlanByIdResponseModel
                {
                    Id = schedulePlan.Id,
                    Code = schedulePlan.Code,
                    SchedulePlanType = schedulePlan.SchedulePlanType,
                    ScheduleId = schedulePlan.ScheduleId,
                    EmployeeId = schedulePlan.SchedulePlanEmployee.EmployeeId,
                    GroupId = schedulePlan.SchedulePlanGroup.GroupId,
                    DepartmentId = schedulePlan.SchedulePlanDepartment.DepartmentId,
                    DateFrom = schedulePlan.DateFrom,
                    IsActive = schedulePlan.IsActive,
                    Notes = schedulePlan.Notes
                }).FirstOrDefaultAsync() ?? throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanNotFound);

            return schedulePlan;

        }
        public async Task<bool> Delete(int schedulePlanId)
        {
            var schedulePlan = await repositoryManager.SchedulePlanRepository.GetEntityByConditionWithTrackingAsync(d => !d.IsDeleted && d.Id == schedulePlanId) ??
                throw new BusinessValidationException(LeillaKeys.SorrySchedulePlanNotFound);

            schedulePlan.Delete();
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task HandleSchedulePlanBackgroundJob()
        {
            try
            {
                var utcDate = DateTime.UtcNow.Date;
                var getNextSchedulePlans = await repositoryManager.SchedulePlanRepository.Get(p => !p.IsDeleted && p.IsActive &&
                p.DateFrom.Date == utcDate)
                    .Select(p => new GetSchedulePlanBackgroundJobLogModel
                    {
                        CompanyId = p.CompanyId,
                        SchedulePlanId = p.Id,
                        ScheduleId = p.ScheduleId,
                        SchedulePlanType = p.SchedulePlanType,
                        EmployeeId = p.SchedulePlanEmployee.EmployeeId,
                        GroupId = p.SchedulePlanGroup.GroupId,
                        DepartmentId = p.SchedulePlanDepartment.DepartmentId
                    }).ToListAsync();

                if (getNextSchedulePlans is not null && getNextSchedulePlans.Count > 0)
                {
                    foreach (var nextSchedulePlan in getNextSchedulePlans)
                    {
                        var startDate = DateTime.UtcNow;
                        switch (nextSchedulePlan.SchedulePlanType)
                        {
                            case SchedulePlanType.Employees:


                                var employeeId = nextSchedulePlan.EmployeeId ?? 0;
                                var getEmployee = await repositoryManager.EmployeeRepository
                                    .GetEntityByConditionWithTrackingAsync(e => !e.IsDeleted && e.Id == employeeId);
                                if (getEmployee != null)
                                {
                                    await HandleSchedulePlanBackgroundJobEmployee(new List<Employee> { getEmployee }, nextSchedulePlan, startDate);
                                }

                                break;
                            case SchedulePlanType.Groups:

                                var groupId = nextSchedulePlan.GroupId ?? 0;
                                var getEmployeesByGroup = await repositoryManager.EmployeeRepository
                                    .Get(e => !e.IsDeleted && e.EmployeeGroups != null &&
                                    e.EmployeeGroups.Any(g => g.GroupId == nextSchedulePlan.GroupId))
                                    .ToListAsync();
                                if (getEmployeesByGroup != null && getEmployeesByGroup.Count > 0)
                                {
                                    await HandleSchedulePlanBackgroundJobEmployee(getEmployeesByGroup, nextSchedulePlan, startDate);
                                }

                                break;
                            case SchedulePlanType.Departments:

                                var departmentId = nextSchedulePlan.DepartmentId ?? 0;
                                var getEmployeesByDepartment = await repositoryManager.EmployeeRepository
                                    .Get(e => !e.IsDeleted && e.DepartmentId == departmentId)
                                    .ToListAsync();
                                if (getEmployeesByDepartment != null && getEmployeesByDepartment.Count > 0)
                                {
                                    await HandleSchedulePlanBackgroundJobEmployee(getEmployeesByDepartment, nextSchedulePlan, startDate);
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }
        }
        public async Task HandleSchedulePlanBackgroundJobEmployee(List<Employee> employees, GetSchedulePlanBackgroundJobLogModel model, DateTime startDate)
        {
            var schedulePlanBackgroundJobLog = new SchedulePlanBackgroundJobLog
            {
                CompanyId = model.CompanyId,
                IsActive = true,
                SchedulePlanId = model.SchedulePlanId,
                SchedulePlanType = model.SchedulePlanType,
                StartDate = startDate
            };

            foreach (var employee in employees)
            {
                schedulePlanBackgroundJobLog.SchedulePlanBackgroundJobLogEmployees.Add(new SchedulePlanBackgroundJobLogEmployee
                {
                    EmployeeId = employee.Id,
                    OldScheduleId = employee.ScheduleId,
                    NewScheduleId = model.ScheduleId,
                    IsActive = true
                });
                employee.ScheduleId = model.ScheduleId;
            }

            schedulePlanBackgroundJobLog.FinishDate = DateTime.UtcNow;
            repositoryManager.SchedulePlanBackgroundJobLogRepository.Insert(schedulePlanBackgroundJobLog);
            await unitOfWork.SaveAsync();
        }
    }
}