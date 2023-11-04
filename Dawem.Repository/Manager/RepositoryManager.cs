using Dawem.Contract.Repository.Attendances.WeekAttendances;
using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Employees;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.Manager;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Repository.Attendances.WeekAttendances;
using Dawem.Repository.Core;
using Dawem.Repository.Core.JustificationsTypes;
using Dawem.Repository.Core.PermissionsTypes;
using Dawem.Repository.Core.Roles;
using Dawem.Repository.Core.VacationsTypes;
using Dawem.Repository.Employees;
using Dawem.Repository.Localization;
using Dawem.Repository.Lookups;
using Dawem.Repository.Others;
using Dawem.Repository.Provider;
using Dawem.Repository.UserManagement;

namespace Dawem.Repository.Manager
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly GeneralSetting generalSetting;
        private readonly RequestInfo requestInfo;

        private IUserRepository userRepository;
        private IActionLogRepository actionLogRepository;
        private IUserBranchRepository userBranchRepository;
        private IBranchRepository branchRepository;
        private IUserTokenRepository userTokenRepository;
        private ICompanyRepository companyRepository;
        private IScreenRepository screenRepository;
        private IUserRoleRepository userRoleRepository;
        private IUserGroupRepository userGroupRepository;
        private ITranslationRepository translationRepository;
        private IEmployeeRepository employeeRepository;
        private IDepartmentRepository departmentRepository;
        private IAssignmentTypeRepository assignmentTypeRepository;
        private ITaskTypeRepository taskTypeRepository;
        private IHolidayTypeRepository holidayTypeRepository;
        private IJustificationsTypeRepository justificationsTypeRepository;
        private IPermissionsTypeRepository permissionsTypeRepository;
        private IVacationsTypeRepository vacationsTypeRepository;
        private IRoleRepository roleRepository;

        private IWeekAttendanceRepository weekAttendanceRepository;
        private IWeekAttendanceShiftRepository weekAttendanceShiftRepository;

        

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
        public IActionLogRepository ActionLogRepository =>
         actionLogRepository ??= new ActionLogRepository(unitOfWork, requestInfo);
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
        public IUserGroupRepository UserGroupRepository =>
        userGroupRepository ??= new UserGroupRepository(unitOfWork, generalSetting);
        public ITranslationRepository TranslationRepository =>
        translationRepository ??= new TranslationRepository(unitOfWork, generalSetting);
        public IEmployeeRepository EmployeeRepository =>
        employeeRepository ??= new EmployeeRepository(unitOfWork, generalSetting);
        public IDepartmentRepository DepartmentRepository =>
        departmentRepository ??= new DepartmentRepository(unitOfWork, generalSetting);

        public IAssignmentTypeRepository AssignmentTypeRepository =>
        assignmentTypeRepository ??= new AssignmentTypeRepository(unitOfWork, generalSetting);
        public ITaskTypeRepository TaskTypeRepository =>
        taskTypeRepository ??= new TaskTypeRepository(unitOfWork, generalSetting);
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



        

        public IWeekAttendanceRepository WeekAttendanceRepository =>
        weekAttendanceRepository ??= new WeekAttendanceRepository(unitOfWork, generalSetting);
        public IWeekAttendanceShiftRepository WeekAttendanceShiftRepository =>
        weekAttendanceShiftRepository ??= new WeekAttendanceShiftRepository(unitOfWork, generalSetting);

    }
}
