using Dawem.Contract.Repository.Core;
using Dawem.Contract.Repository.Localization;
using Dawem.Contract.Repository.Lookups;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Provider;
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
        IUserGroupRepository UserGroupRepository { get; }
        ITranslationRepository TranslationRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IJustificationsTypeRepository JustificationsTypeRepository { get; }

    }
}