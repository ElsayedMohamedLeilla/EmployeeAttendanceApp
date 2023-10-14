using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;

namespace Dawem.Contract.Repository.Manager;
public interface IRepositoryManager
{
    ICompanyRepository CompanyRepository { get; }
    IUserRepository UserRepository { get; }
    IActionLogRepository ActionLogRepository { get; }
    IUserBranchRepository UserBranchRepository { get; }
    IBranchRepository BranchRepository { get; }
    IUserTokenRepository UserTokenRepository { get; }
}
