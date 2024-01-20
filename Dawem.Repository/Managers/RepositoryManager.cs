using Dawem.Contract.Repository.Attendances;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Permissions;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.Requests;
using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Repository.Attendances;
using Dawem.Repository.Core;
using Dawem.Repository.Core.Groups;
using Dawem.Repository.Core.Holidays;
using Dawem.Repository.Core.JustificationsTypes;
using Dawem.Repository.Core.NotificationsStores;
using Dawem.Repository.Core.PermissionsTypes;
using Dawem.Repository.Core.Roles;
using Dawem.Repository.Core.VacationsTypes;
using Dawem.Repository.Employees;
using Dawem.Repository.Localizations;
using Dawem.Repository.Lookups;
using Dawem.Repository.Others;
using Dawem.Repository.Providers;
using Dawem.Repository.Requests;
using Dawem.Repository.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Repository.Schedules.SchedulePlans;
using Dawem.Repository.Schedules.Schedules;
using Dawem.Repository.UserManagement;

namespace Dawem.Repository.Managers
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly GeneralSetting generalSetting;
        private readonly RequestInfo requestInfo;
        private IUserRepository userRepository;
        private IPermissionLogRepository permissionLogRepository;
        private IPermissionRepository permissionRepository;
        private IPermissionScreenRepository permissionScreenRepository;
        private IPermissionScreenActionRepository permissionScreenActionRepository;
        private IUserBranchRepository userBranchRepository;
        private IBranchRepository branchRepository;
        private IUserTokenRepository userTokenRepository;
        private ICompanyRepository companyRepository;
        private IScreenRepository screenRepository;
        private IUserRoleRepository userRoleRepository;
        private ITranslationRepository translationRepository;
        private IEmployeeRepository employeeRepository;
        private IDepartmentRepository departmentRepository;
        private IAssignmentTypeRepository assignmentTypeRepository;
        private ITaskTypeRepository taskTypeRepository;
        private IJobTitleRepository jobTitleRepository;
        private IHolidayTypeRepository holidayTypeRepository;
        private IJustificationsTypeRepository justificationsTypeRepository;
        private IPermissionsTypeRepository permissionsTypeRepository;
        private IVacationsTypeRepository vacationsTypeRepository;
        private IRoleRepository roleRepository;
        private IScheduleRepository scheduleRepository;
        private ISchedulePlanRepository schedulePlanRepository;
        private ISchedulePlanEmployeeRepository schedulePlanEmployeeRepository;
        private ISchedulePlanGroupRepository schedulePlanGroupRepository;
        private ISchedulePlanDepartmentRepository schedulePlanDepartmentRepository;
        private ISchedulePlanBackgroundJobLogRepository schedulePlanBackgroundJobLogRepository;
        private ISchedulePlanBackgroundJobLogEmployeeRepository schedulePlanBackgroundJobLogEmployeeRepository;
        private IScheduleDayRepository scheduleDayRepository;
        private IShiftWorkingTimeRepository shiftWorkingTimeRepository;
        private IGroupRepository groupRepository;
        private IGroupEmployeeRepository groupEmployeeRepository;
        private IGroupManagerDelegatorRepository groupManagerDelegatorRepository;
        private IEmployeeAttendanceRepository employeeAttendanceRepository;
        private IEmployeeAttendanceCheckRepository employeeAttendanceCheckRepository;
        private IDepartmentManagerDelegatorRepository departmentManagerDelegatorRepository;
        private IZoneDepartmentRepository departmentZoneRepository;
        private IZoneEmployeeRepository employeeZoneRepository;
        private IZoneGroupRepository groupZoneRepository;
        private IZoneRepository zoneRepository;
        private IFingerprintDeviceRepository fingerprintDeviceRepository;
        private IRequestRepository requestRepository;
        private IRequestAssignmentRepository requestAssignmentRepository;
        private IRequestJustificationRepository requestJustificationRepository;
        private IRequestTaskRepository requestTaskRepository;
        private IRequestPermissionRepository requestPermissionRepository;
        private IRequestVacationRepository requestVacationRepository;
        private IRequestAttachmentRepository requestAttachmentRepository;
        private IRequestTaskEmployeeRepository requestTaskEmployeeRepository;
        private IHolidayRepository holidayRepository;
        private INotificationStoreRepository notificationStoreRepository;
        private IVacationBalanceRepository vacationBalanceRepository;
        private IFingerprintEnforcementRepository fingerprintEnforcementRepository;
        private IFingerprintEnforcementNotifyWayRepository fingerprintEnforcementNotifyWayRepository;
        private IFingerprintEnforcementEmployeeRepository fingerprintEnforcementEmployeeRepository;
        private IFingerprintEnforcementGroupRepository fingerprintEnforcementGroupRepository;
        private IFingerprintEnforcementActionRepository fingerprintEnforcementActionRepository;
        private IFingerprintEnforcementDepartmentRepository fingerprintEnforcementDepartmentRepository;
        private INonComplianceActionRepository nonComplianceActionRepository;
        private INotificationUserRepository notificationUserRepository;
        private INotificationUserDeviceTokenRepository notificationUserDeviceTokenRepository;


        public RepositoryManager(IUnitOfWork<ApplicationDBContext> _unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            generalSetting = _generalSetting;
            requestInfo = _requestHeaderContext;
        }

        public ICompanyRepository CompanyRepository =>
         companyRepository ??= new CompanyRepository(unitOfWork, generalSetting);
        public IUserRepository UserRepository =>
         userRepository ??= new UserRepository(requestInfo, unitOfWork, generalSetting);
        public IPermissionRepository PermissionRepository =>
         permissionRepository ??= new PermissionRepository(unitOfWork, requestInfo);
        public IPermissionScreenRepository PermissionScreenRepository =>
         permissionScreenRepository ??= new PermissionScreenRepository(unitOfWork);
        public IPermissionScreenActionRepository PermissionScreenActionRepository =>
         permissionScreenActionRepository ??= new PermissionScreenActionRepository(unitOfWork);
        public IPermissionLogRepository PermissionLogRepository =>
         permissionLogRepository ??= new PermissionLogRepository(unitOfWork, requestInfo);
        public IUserBranchRepository UserBranchRepository =>
        userBranchRepository ??= new UserBranchRepository(unitOfWork, generalSetting);
        public IBranchRepository BranchRepository =>
         branchRepository ??= new BranchRepository(unitOfWork, requestInfo, generalSetting);
        public IUserTokenRepository UserTokenRepository =>
         userTokenRepository ??= new UserTokenRepository(unitOfWork, generalSetting);
        public IScreenRepository ScreenRepository =>
        screenRepository ??= new ScreenRepository(unitOfWork, generalSetting);
        public IUserRoleRepository UserRoleRepository =>
        userRoleRepository ??= new UserRoleRepository(unitOfWork, generalSetting);
        public ITranslationRepository TranslationRepository =>
        translationRepository ??= new TranslationRepository(unitOfWork, generalSetting);
        public IEmployeeRepository EmployeeRepository =>
        employeeRepository ??= new EmployeeRepository(unitOfWork, generalSetting, requestInfo);
        public IDepartmentRepository DepartmentRepository =>
        departmentRepository ??= new DepartmentRepository(unitOfWork, generalSetting);
        public IAssignmentTypeRepository AssignmentTypeRepository =>
        assignmentTypeRepository ??= new AssignmentTypeRepository(unitOfWork, generalSetting);
        public ITaskTypeRepository TaskTypeRepository =>
        taskTypeRepository ??= new TaskTypeRepository(unitOfWork, generalSetting);
        public IJobTitleRepository JobTitleRepository =>
        jobTitleRepository ??= new JobTitleRepository(unitOfWork, generalSetting);
        public IHolidayTypeRepository HolidayTypeRepository =>
        holidayTypeRepository ??= new HolidayTypeRepository(unitOfWork, generalSetting);
        public IJustificationsTypeRepository JustificationsTypeRepository =>
        justificationsTypeRepository ??= new JustificationsTypeRepository(unitOfWork, generalSetting);
        public IVacationsTypeRepository VacationsTypeRepository =>
        vacationsTypeRepository ??= new VacationsTypeRepository(unitOfWork, generalSetting);
        public IPermissionsTypeRepository PermissionsTypeRepository =>
        permissionsTypeRepository ??= new PermissionsTypeRepository(unitOfWork, generalSetting);
        public IRoleRepository RoleRepository =>
        roleRepository ??= new RoleRepository(unitOfWork, generalSetting);
        public IShiftWorkingTimeRepository ShiftWorkingTimeRepository =>
        shiftWorkingTimeRepository ??= new ShiftWorkingTimeRepository(unitOfWork, generalSetting);
        public IScheduleRepository ScheduleRepository =>
        scheduleRepository ??= new ScheduleRepository(unitOfWork, generalSetting);
        public IScheduleDayRepository ScheduleDayRepository =>
        scheduleDayRepository ??= new ScheduleDayRepository(unitOfWork, generalSetting);
        public IGroupRepository GroupRepository =>
        groupRepository ??= new GroupRepository(unitOfWork, generalSetting);
        public ISchedulePlanRepository SchedulePlanRepository =>
         schedulePlanRepository ??= new SchedulePlanRepository(unitOfWork, generalSetting);
        public ISchedulePlanEmployeeRepository SchedulePlanEmployeeRepository =>
         schedulePlanEmployeeRepository ??= new SchedulePlanEmployeeRepository(unitOfWork, generalSetting);
        public ISchedulePlanGroupRepository SchedulePlanGroupRepository =>
        schedulePlanGroupRepository ??= new SchedulePlanGroupRepository(unitOfWork, generalSetting);
        public ISchedulePlanDepartmentRepository SchedulePlanDepartmentRepository =>
        schedulePlanDepartmentRepository ??= new SchedulePlanDepartmentRepository(unitOfWork, generalSetting);
        public ISchedulePlanBackgroundJobLogRepository SchedulePlanLogRepository =>
         schedulePlanBackgroundJobLogRepository ??= new SchedulePlanBackgroundJobLogRepository(unitOfWork, generalSetting);
        public ISchedulePlanBackgroundJobLogEmployeeRepository SchedulePlanLogEmployeeRepository =>
         schedulePlanBackgroundJobLogEmployeeRepository ??= new SchedulePlanBackgroundJobLogEmployeeRepository(unitOfWork, generalSetting);
        public IGroupEmployeeRepository GroupEmployeeRepository =>
        groupEmployeeRepository ??= new GroupEmployeeRepository(unitOfWork, generalSetting);
        public IGroupManagerDelegatorRepository GroupManagerDelegatorRepository =>
        groupManagerDelegatorRepository ??= new GroupManagerDelegatorRepository(unitOfWork, generalSetting);
        public IEmployeeAttendanceRepository EmployeeAttendanceRepository =>
        employeeAttendanceRepository ??= new EmployeeAttendanceRepository(unitOfWork, generalSetting, requestInfo);
        public IEmployeeAttendanceCheckRepository EmployeeAttendanceCheckRepository =>
        employeeAttendanceCheckRepository ??= new EmployeeAttendanceCheckRepository(unitOfWork, generalSetting, requestInfo);

        public IDepartmentManagerDelegatorRepository DepartmentManagerDelegatorRepository =>
        departmentManagerDelegatorRepository ??= new DepartmentManagerDelegatorRepository(unitOfWork, generalSetting);
        public IZoneDepartmentRepository ZoneDepartmentRepository =>
        departmentZoneRepository ??= new ZoneDepartmentRepository(unitOfWork, generalSetting);

        public IZoneEmployeeRepository ZoneEmployeeRepository =>
        employeeZoneRepository ??= new EmployeeZoneRepository(unitOfWork, generalSetting);
        public IZoneGroupRepository ZoneGroupRepository =>
        groupZoneRepository ??= new GroupZoneRepository(unitOfWork, generalSetting);

        public IZoneRepository ZoneRepository =>
        zoneRepository ??= new ZoneRepository(unitOfWork, generalSetting);

        public IFingerprintDeviceRepository FingerprintDeviceRepository =>
        fingerprintDeviceRepository ??= new FingerprintDeviceRepository(unitOfWork, generalSetting, requestInfo);

        public IRequestRepository RequestRepository =>
            requestRepository ??= new RequestRepository(unitOfWork, generalSetting, requestInfo);

        public IRequestAssignmentRepository RequestAssignmentRepository =>
            requestAssignmentRepository ??= new RequestAssignmentRepository(unitOfWork, generalSetting, requestInfo);

        public IRequestAttachmentRepository RequestAttachmentRepository =>
            requestAttachmentRepository ??= new RequestAttachmentRepository(unitOfWork, generalSetting);

        public IRequestJustificationRepository RequestJustificationRepository =>
            requestJustificationRepository ??= new RequestJustificationRepository(unitOfWork, generalSetting, requestInfo);

        public IRequestPermissionRepository RequestPermissionRepository =>
            requestPermissionRepository ??= new RequestPermissionRepository(unitOfWork, generalSetting, requestInfo);

        public IRequestTaskRepository RequestTaskRepository =>
            requestTaskRepository ??= new RequestTaskRepository(unitOfWork, generalSetting, requestInfo);

        public IRequestTaskEmployeeRepository RequestTaskEmployeeRepository =>
            requestTaskEmployeeRepository ??= new RequestTaskEmployeeRepository(unitOfWork, generalSetting);

        public IRequestVacationRepository RequestVacationRepository =>
            requestVacationRepository ??= new RequestVacationRepository(unitOfWork, generalSetting, requestInfo);

        public IVacationBalanceRepository VacationBalanceRepository =>
            vacationBalanceRepository ??= new VacationBalanceRepository(unitOfWork, generalSetting);


        public IHolidayRepository HolidayRepository =>
          holidayRepository ??= new HolidayRepository(unitOfWork, generalSetting);

        public INotificationStoreRepository NotificationStoreRepository =>
            notificationStoreRepository ??= new NotificationStoreRepository(unitOfWork, generalSetting);

        public IFingerprintEnforcementRepository FingerprintEnforcementRepository =>
            fingerprintEnforcementRepository ??= new FingerprintEnforcementRepository(unitOfWork, generalSetting);

        public IFingerprintEnforcementNotifyWayRepository FingerprintEnforcementNotifyWayRepository =>
            fingerprintEnforcementNotifyWayRepository ??= new FingerprintEnforcementNotifyWayRepository(unitOfWork, generalSetting);
        public IFingerprintEnforcementEmployeeRepository FingerprintEnforcementEmployeeRepository =>
            fingerprintEnforcementEmployeeRepository ??= new FingerprintEnforcementEmployeeRepository(unitOfWork, generalSetting);
        public IFingerprintEnforcementDepartmentRepository FingerprintEnforcementDepartmentRepository =>
            fingerprintEnforcementDepartmentRepository ??= new FingerprintEnforcementDepartmentRepository(unitOfWork, generalSetting);
        public IFingerprintEnforcementActionRepository FingerprintEnforcementActionRepository =>
            fingerprintEnforcementActionRepository ??= new FingerprintEnforcementActionRepository(unitOfWork, generalSetting);
        public IFingerprintEnforcementGroupRepository FingerprintEnforcementGroupRepository =>
            fingerprintEnforcementGroupRepository ??= new FingerprintEnforcementGroupRepository(unitOfWork, generalSetting);
        public INonComplianceActionRepository NonComplianceActionRepository =>
            nonComplianceActionRepository ??= new NonComplianceActionRepository(unitOfWork, generalSetting);
        public INotificationUserRepository NotificationUserRepository =>
            notificationUserRepository ??= new NotificationUserRepository(unitOfWork, generalSetting);
        public INotificationUserDeviceTokenRepository NotificationUserDeviceTokenRepository =>
            notificationUserDeviceTokenRepository ??= new NotificationUserDeviceTokenRepository(unitOfWork, generalSetting);
    }
}
