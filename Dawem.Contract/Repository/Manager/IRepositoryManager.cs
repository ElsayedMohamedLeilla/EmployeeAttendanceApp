using Dawem.Contract.Repository.Attendances;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.Requests;
using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Contract.Repository.UserManagement;

namespace Dawem.Contract.Repository.Manager
{
    public interface IRepositoryManager
    {
        ICompanyRepository CompanyRepository { get; }
        IUserRepository UserRepository { get; }
        IActionLogRepository ActionLogRepository { get; }
        IUserBranchRepository UserBranchRepository { get; }
        IBranchRepository BranchRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IScreenRepository ScreenRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        ITranslationRepository TranslationRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IJustificationsTypeRepository JustificationsTypeRepository { get; }
        IVacationsTypeRepository VacationsTypeRepository { get; }
        IPermissionsTypeRepository PermissionsTypeRepository { get; }
        IAssignmentTypeRepository AssignmentTypeRepository { get; }
        ITaskTypeRepository TaskTypeRepository { get; }
        IJobTitleRepository JobTitleRepository { get; }
        IHolidayTypeRepository HolidayTypeRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        ISchedulePlanRepository SchedulePlanRepository { get; }
        ISchedulePlanEmployeeRepository SchedulePlanEmployeeRepository { get; }
        ISchedulePlanGroupRepository SchedulePlanGroupRepository { get; }
        ISchedulePlanDepartmentRepository SchedulePlanDepartmentRepository { get; }
        ISchedulePlanBackgroundJobLogRepository SchedulePlanBackgroundJobLogRepository { get; }
        ISchedulePlanBackgroundJobLogEmployeeRepository SchedulePlanBackgroundJobLogEmployeeRepository { get; }
        IEmployeeAttendanceRepository EmployeeAttendanceRepository { get; }
        IEmployeeAttendanceCheckRepository EmployeeAttendanceCheckRepository { get; }        
        IScheduleDayRepository ScheduleDayRepository { get; }
        IRoleRepository RoleRepository { get; }
        IShiftWorkingTimeRepository ShiftWorkingTimeRepository { get; }
        IGroupRepository GroupRepository { get; }
        IGroupEmployeeRepository GroupEmployeeRepository { get; }
        IGroupManagerDelegatorRepository GroupManagerDelegatorRepository { get; }
        IDepartmentManagerDelegatorRepository DepartmentManagerDelegatorRepository { get; }
        IZoneDepartmentRepository ZoneDepartmentRepository { get; }
        IZoneEmployeeRepository ZoneEmployeeRepository { get; }
        IZoneGroupRepository ZoneGroupRepository { get; }
        IZoneRepository ZoneRepository { get; }
        IFingerprintDeviceRepository FingerprintDeviceRepository  { get; }
        IRequestRepository RequestRepository { get; }
        IRequestAssignmentRepository RequestAssignmentRepository { get; }
        IRequestAttachmentRepository RequestAttachmentRepository { get; }
        IRequestJustificationRepository RequestJustificationRepository { get; }
        IRequestPermissionRepository RequestPermissionRepository { get; }
        IRequestTaskRepository RequestTaskRepository { get; }
        IRequestTaskEmployeeRepository RequestTaskEmployeeRepository { get; }
        IRequestVacationRepository RequestVacationRepository { get; }
    }
}