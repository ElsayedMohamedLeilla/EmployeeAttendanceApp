using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.UserManagement;

namespace Dawem.Contract.Repository.Manager;
public interface IRepositoryManager
{

    IUserRepository UserRepository { get; }
    IActionLogRepository ActionLogRepository { get; }

}
