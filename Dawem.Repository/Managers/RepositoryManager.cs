using Dawem.Contract.BusinessLogic.AdminPanel.DefaultLookups;
using Dawem.Contract.RealTime.Firebase;
using Dawem.Contract.Repository.Attendances;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Core.DefaultLookups;
using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Contract.Repository.MenuItems;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Permissions;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.Requests;
using Dawem.Contract.Repository.Schedules.SchedulePlans;
using Dawem.Contract.Repository.Schedules.Schedules;
using Dawem.Contract.Repository.Settings;
using Dawem.Contract.Repository.Subscriptions;
using Dawem.Contract.Repository.Summons;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Repository.Attendances;
using Dawem.Repository.Core;
using Dawem.Repository.Core.DefaultLookups;
using Dawem.Repository.Core.Groups;
using Dawem.Repository.Core.Holidays;
using Dawem.Repository.Core.JustificationsTypes;
using Dawem.Repository.Core.Notifications;
using Dawem.Repository.Core.PermissionsTypes;
using Dawem.Repository.Core.Roles;
using Dawem.Repository.Core.VacationsTypes;
using Dawem.Repository.Employees;
using Dawem.Repository.Localizations;
using Dawem.Repository.Lookups;
using Dawem.Repository.MenuItems;
using Dawem.Repository.Others;
using Dawem.Repository.Providers;
using Dawem.Repository.RealTime.Firebase;
using Dawem.Repository.Requests;
using Dawem.Repository.Schedules.SchedulePlanBackgroundJobLogs;
using Dawem.Repository.Schedules.SchedulePlans;
using Dawem.Repository.Schedules.Schedules;
using Dawem.Repository.Summons;
using Dawem.Repository.UserManagement;

namespace Dawem.Repository.Managers
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly ApplicationDBContext context;

        private readonly GeneralSetting generalSetting;
        private readonly RequestInfo requestInfo;
        private IUserRepository userRepository;
        private ILanguageRepository languageRepository;
        private IPermissionLogRepository permissionLogRepository;
        private IPermissionRepository permissionRepository;
        private IPermissionScreenRepository permissionScreenRepository;
        private IPermissionScreenActionRepository permissionScreenActionRepository;
        private IUserBranchRepository userBranchRepository;
        private ICompanyBranchRepository companyBranchRepository;
        private ICompanyAttachmentRepository companyAttachmentRepository;
        private ICompanyIndustryRepository companyIndustryRepository;
        private IUserTokenRepository userTokenRepository;
        private ICompanyRepository companyRepository;
        private ISubscriptionRepository subscriptionRepository;
        private ISubscriptionLogRepository subscriptionLogRepository;
        private ISubscriptionPaymentRepository subscriptionPaymentRepository;
        private IPlanRepository planRepository;
        private IPlanNameTranslationRepository planNameTranslationRepository;
        private ISettingRepository dawemSettingRepository;
        private ICountryRepository countryRepository;
        private ICurrencyRepository currencyRepository;
        private IUserRoleRepository userRoleRepository;
        private ITranslationRepository translationRepository;
        private IEmployeeRepository employeeRepository;
        private IDepartmentRepository departmentRepository;
        private IAssignmentTypeRepository assignmentTypeRepository;
        private ITaskTypeRepository taskTypeRepository;
        private IJobTitleRepository jobTitleRepository;
        private IHolidayTypeRepository holidayTypeRepository;
        private IJustificationsTypeRepository justificationsTypeRepository;
        private IResponsibilityRepository responsibilityRepository;
        private IUserResponsibilityRepository userResponsibilityRepository;
        private IPermissionsTypeRepository permissionsTypeRepository;
        private IVacationsTypeRepository vacationsTypeRepository;
        private IRoleRepository roleRepository;
        private IScheduleRepository scheduleRepository;
        private ISchedulePlanRepository schedulePlanRepository;
        private ISchedulePlanEmployeeRepository schedulePlanEmployeeRepository;
        private ISchedulePlanGroupRepository schedulePlanGroupRepository;
        private ISchedulePlanDepartmentRepository schedulePlanDepartmentRepository;
        private ISchedulePlanBackgroundJobLogRepository schedulePlanLogRepository;
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
        private INotificationRepository notificationRepository;
        private IVacationBalanceRepository vacationBalanceRepository;
        private ISummonRepository summonRepository;
        private ISummonLogSanctionRepository summonLogSanctionRepository;
        private ISummonLogRepository summonLogRepository;
        private ISummonNotifyWayRepository summonNotifyWayRepository;
        private ISummonEmployeeRepository summonEmployeeRepository;
        private ISummonGroupRepository summonGroupRepository;
        private ISummonSanctionRepository summonSanctionRepository;
        private ISummonDepartmentRepository summonDepartmentRepository;
        private ISanctionRepository sanctionRepository;
        private INotificationUserRepository notificationUserRepository;
        private INotificationUserFCMTokenRepository notificationUserFCMTokenRepository;
        private IEmployeeOTPRepository employeeOTPRepository;
        private IMenuItemRepository menuItemRepository;
        private IMenuItemActionRepository menuItemActionRepository;
        private IMenuItemNameTranslationRepository menuItemNameTranslationRepository;
        private IPlanScreenRepository planScreenRepository;
        private IOldScreenRepository oldScreenRepository;
        private IDefaultLookupsNameTranslationRepository defaultLookupsNameTranslationRepository;

        private IDefaultVacationTypeRepository defaultVacationTypeRepository;
        private IDefaultShiftTypeRepository defaultShiftTypeRepository;
        private IDefaultJustificationTypeRepository defaultJustificationTypeRepository;
        private IDefaultTaskTypeRepository defaultTaskTypeRepository;
        private IDefaultPermissionTypeRepository defaultPermissionTypeRepository;
        private IDefaultOfficialHolidayRepository defaultOfficialHolidayRepository;

        private IDefaultDepartmentsRepository defaultDepartmentsRepository;
        private IDefaultJobTitlesRepository defaultJobTitlesRepository;
        private IDefaultPenaltiesRepository defaultPenaltiesRepository;



        public RepositoryManager(IUnitOfWork<ApplicationDBContext> _unitOfWork, GeneralSetting _generalSetting, RequestInfo _requestInfo)
        {
            unitOfWork = _unitOfWork;
            generalSetting = _generalSetting;
            requestInfo = _requestInfo;
        }
        public ICountryRepository CountryRepository =>
            countryRepository ??= new CountryRepository(unitOfWork, generalSetting);
        public ILanguageRepository LanguageRepository =>
            languageRepository ??= new LanguageRepository(unitOfWork, generalSetting);
        public ICurrencyRepository CurrencyRepository =>
            currencyRepository ??= new CurrencyRepository(unitOfWork, generalSetting);
        public ICompanyRepository CompanyRepository =>
         companyRepository ??= new CompanyRepository(unitOfWork, generalSetting);
        public ICompanyBranchRepository CompanyBranchRepository =>
         companyBranchRepository ??= new CompanyBranchRepository(unitOfWork, requestInfo, generalSetting);
        public ICompanyAttachmentRepository CompanyAttachmentRepository =>
         companyAttachmentRepository ??= new CompanyAttachmentRepository(unitOfWork, generalSetting);
        public ICompanyIndustryRepository CompanyIndustryRepository =>
         companyIndustryRepository ??= new CompanyIndustryRepository(unitOfWork, generalSetting);
        public ISubscriptionRepository SubscriptionRepository =>
        subscriptionRepository ??= new SubscriptionRepository(unitOfWork, generalSetting);

        public ISubscriptionLogRepository SubscriptionLogRepository =>
        subscriptionLogRepository ??= new SubscriptionLogRepository(unitOfWork, generalSetting);

        public ISubscriptionPaymentRepository SubscriptionPaymentRepository =>
        subscriptionPaymentRepository ??= new SubscriptionPaymentRepository(unitOfWork, generalSetting);
        public IPlanRepository PlanRepository =>
        planRepository ??= new PlanRepository(unitOfWork, generalSetting, requestInfo);
        public IPlanNameTranslationRepository PlanNameTranslationRepository =>
        planNameTranslationRepository ??= new PlanNameTranslationRepository(unitOfWork, generalSetting, requestInfo);



        public ISettingRepository SettingRepository =>
         dawemSettingRepository ??= new SettingRepository(unitOfWork, generalSetting);
        public IUserRepository UserRepository =>
         userRepository ??= new UserRepository(requestInfo, unitOfWork, generalSetting);
        public IPermissionRepository PermissionRepository =>
         permissionRepository ??= new PermissionRepository(unitOfWork, requestInfo);
        public IPermissionScreenRepository PermissionScreenRepository =>
         permissionScreenRepository ??= new PermissionScreenRepository(unitOfWork, requestInfo);
        public IPermissionScreenActionRepository PermissionScreenActionRepository =>
         permissionScreenActionRepository ??= new PermissionScreenActionRepository(unitOfWork);
        public IPermissionLogRepository PermissionLogRepository =>
         permissionLogRepository ??= new PermissionLogRepository(unitOfWork, requestInfo);
        public IUserBranchRepository UserBranchRepository =>
        userBranchRepository ??= new UserBranchRepository(unitOfWork, generalSetting);
        public IUserTokenRepository UserTokenRepository =>
         userTokenRepository ??= new UserTokenRepository(unitOfWork, generalSetting);
        public IMenuItemRepository MenuItemRepository =>
        menuItemRepository ??= new MenuItemRepository(unitOfWork, generalSetting, requestInfo);
        public IMenuItemActionRepository MenuItemActionRepository =>
        menuItemActionRepository ??= new MenuItemActionRepository(unitOfWork, generalSetting, requestInfo);
        public IMenuItemNameTranslationRepository MenuItemNameTranslationRepository =>
        menuItemNameTranslationRepository ??= new MenuItemNameTranslationRepository(unitOfWork, generalSetting, requestInfo);
        public IPlanScreenRepository PlanScreenRepository =>
        planScreenRepository ??= new PlanScreenRepository(unitOfWork, generalSetting, requestInfo);
        public IUserRoleRepository UserRoleRepository =>
        userRoleRepository ??= new UserRoleRepository(unitOfWork, generalSetting);
        public ITranslationRepository TranslationRepository =>
        translationRepository ??= new TranslationRepository(unitOfWork, generalSetting);
        public IEmployeeRepository EmployeeRepository =>
        employeeRepository ??= new EmployeeRepository(userRepository, unitOfWork, generalSetting, requestInfo);
        public IDepartmentRepository DepartmentRepository =>
        departmentRepository ??= new DepartmentRepository(unitOfWork, generalSetting, requestInfo);
        public IAssignmentTypeRepository AssignmentTypeRepository =>
        assignmentTypeRepository ??= new AssignmentTypeRepository(unitOfWork, generalSetting, requestInfo);
        public ITaskTypeRepository TaskTypeRepository =>
        taskTypeRepository ??= new TaskTypeRepository(unitOfWork, generalSetting, requestInfo);
        public IJobTitleRepository JobTitleRepository =>
        jobTitleRepository ??= new JobTitleRepository(unitOfWork, generalSetting, requestInfo);
        public IHolidayTypeRepository HolidayTypeRepository =>
        holidayTypeRepository ??= new HolidayTypeRepository(unitOfWork, generalSetting, requestInfo);
        public IJustificationsTypeRepository JustificationsTypeRepository =>
        justificationsTypeRepository ??= new JustificationsTypeRepository(unitOfWork, generalSetting, requestInfo);
        public IResponsibilityRepository ResponsibilityRepository =>
        responsibilityRepository ??= new ResponsibilityRepository(unitOfWork, generalSetting, requestInfo);
        public IUserResponsibilityRepository UserResponsibilityRepository =>
        userResponsibilityRepository ??= new UserResponsibilityRepository(unitOfWork, generalSetting, requestInfo);
        public IVacationsTypeRepository VacationsTypeRepository =>
        vacationsTypeRepository ??= new VacationsTypeRepository(unitOfWork, generalSetting, requestInfo);
        public IPermissionsTypeRepository PermissionsTypeRepository =>
        permissionsTypeRepository ??= new PermissionsTypeRepository(unitOfWork, generalSetting, requestInfo);
        public IRoleRepository RoleRepository =>
        roleRepository ??= new RoleRepository(unitOfWork, generalSetting);
        public IShiftWorkingTimeRepository ShiftWorkingTimeRepository =>
        shiftWorkingTimeRepository ??= new ShiftWorkingTimeRepository(unitOfWork, generalSetting, requestInfo);
        public IScheduleRepository ScheduleRepository =>
        scheduleRepository ??= new ScheduleRepository(unitOfWork, generalSetting, requestInfo);
        public IScheduleDayRepository ScheduleDayRepository =>
        scheduleDayRepository ??= new ScheduleDayRepository(unitOfWork, generalSetting);
        public IGroupRepository GroupRepository =>
        groupRepository ??= new GroupRepository(unitOfWork, generalSetting, requestInfo);
        public ISchedulePlanRepository SchedulePlanRepository =>
         schedulePlanRepository ??= new SchedulePlanRepository(unitOfWork, generalSetting, requestInfo);
        public ISchedulePlanEmployeeRepository SchedulePlanEmployeeRepository =>
         schedulePlanEmployeeRepository ??= new SchedulePlanEmployeeRepository(unitOfWork, generalSetting);
        public ISchedulePlanGroupRepository SchedulePlanGroupRepository =>
        schedulePlanGroupRepository ??= new SchedulePlanGroupRepository(unitOfWork, generalSetting);
        public ISchedulePlanDepartmentRepository SchedulePlanDepartmentRepository =>
        schedulePlanDepartmentRepository ??= new SchedulePlanDepartmentRepository(unitOfWork, generalSetting);
        public ISchedulePlanBackgroundJobLogRepository SchedulePlanLogRepository =>
         schedulePlanLogRepository ??= new SchedulePlanBackgroundJobLogRepository(unitOfWork, generalSetting, requestInfo);
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
        zoneRepository ??= new ZoneRepository(unitOfWork, generalSetting, requestInfo);

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
            vacationBalanceRepository ??= new VacationBalanceRepository(unitOfWork, generalSetting, requestInfo);


        public IHolidayRepository HolidayRepository =>
          holidayRepository ??= new HolidayRepository(unitOfWork, generalSetting, requestInfo);

        public INotificationRepository NotificationRepository =>
            notificationRepository ??= new NotificationRepository(unitOfWork, generalSetting, requestInfo);

        public ISummonRepository SummonRepository =>
            summonRepository ??= new SummonRepository(unitOfWork, generalSetting, requestInfo);

        public ISummonLogSanctionRepository SummonLogSanctionRepository =>
            summonLogSanctionRepository ??= new SummonLogSanctionRepository(unitOfWork, generalSetting, requestInfo);

        public ISummonLogRepository SummonLogRepository =>
            summonLogRepository ??= new SummonLogRepository(unitOfWork, generalSetting, requestInfo);

        public ISummonNotifyWayRepository SummonNotifyWayRepository =>
            summonNotifyWayRepository ??= new SummonNotifyWayRepository(unitOfWork, generalSetting);
        public ISummonEmployeeRepository SummonEmployeeRepository =>
            summonEmployeeRepository ??= new SummonEmployeeRepository(unitOfWork, generalSetting);
        public ISummonDepartmentRepository SummonDepartmentRepository =>
            summonDepartmentRepository ??= new SummonDepartmentRepository(unitOfWork, generalSetting);
        public ISummonSanctionRepository SummonSanctionRepository =>
            summonSanctionRepository ??= new SummonActionRepository(unitOfWork, generalSetting);
        public ISummonGroupRepository SummonGroupRepository =>
            summonGroupRepository ??= new SummonGroupRepository(unitOfWork, generalSetting);
        public ISanctionRepository SanctionRepository =>
            sanctionRepository ??= new SanctionRepository(unitOfWork, generalSetting, requestInfo);
        public INotificationUserRepository NotificationUserRepository =>
            notificationUserRepository ??= new NotificationUserRepository(unitOfWork, generalSetting);
        public INotificationUserFCMTokenRepository NotificationUserFCMTokenRepository =>
            notificationUserFCMTokenRepository ??= new NotificationUserFCMTokenRepository(unitOfWork, generalSetting);
        public IEmployeeOTPRepository EmployeeOTPRepository =>
           employeeOTPRepository ??= new EmployeeOTPRepository(unitOfWork, generalSetting);
        public IOldScreenRepository OldScreenRepository =>
           oldScreenRepository ??= new OldScreenRepository(unitOfWork, generalSetting);

        public IDefaultLookupsNameTranslationRepository DefaultLookupsNameTranslationRepository =>
          defaultLookupsNameTranslationRepository ??= new DefaultLookupsNameTranslationRepository(unitOfWork, generalSetting, requestInfo);

        public IDefaultVacationTypeRepository DefaultVacationTypeRepository =>
          defaultVacationTypeRepository ??= new DefaultVacationTypeRepository(unitOfWork, generalSetting, requestInfo);
        public IDefaultShiftTypeRepository DefaultShiftTypeRepository =>
          defaultShiftTypeRepository ??= new DefaultShiftTypeRepository(unitOfWork, generalSetting, requestInfo);

        public IDefaultJustificationTypeRepository DefaultJustificationTypeRepository =>
         defaultJustificationTypeRepository ??= new DefaultJustificationTypeRepository(unitOfWork, generalSetting, requestInfo);

        public IDefaultPermissionTypeRepository DefaultPermissionTypeRepository =>
        defaultPermissionTypeRepository ??= new DefaultPermissionTypeRepository(unitOfWork, generalSetting, requestInfo);

        public IDefaultTaskTypeRepository DefaultTaskTypeRepository =>
        defaultTaskTypeRepository ??= new DefaultTaskTypeRepository(unitOfWork, generalSetting, requestInfo);

        public IDefaultOfficialHolidayRepository DefaultOfficialHolidayRepository =>
        defaultOfficialHolidayRepository ??= new DefaultOfficialHolidayRepository(unitOfWork, generalSetting, requestInfo);

        public IDefaultDepartmentsRepository DefaultDepartmentsRepository =>
            defaultDepartmentsRepository ??= new DefaultDepartmentsRepository(unitOfWork, generalSetting, requestInfo);
        public IDefaultJobTitlesRepository DefaultJobTitlesRepository =>
           defaultJobTitlesRepository ??= new DefaultJobTitlesRepository(unitOfWork, generalSetting, requestInfo);
        public IDefaultPenaltiesRepository DefaultPenaltiesRepository =>
           defaultPenaltiesRepository ??= new DefaultPenaltiesRepository(unitOfWork, generalSetting, requestInfo);

    }
}
