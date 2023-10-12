using Dawem.Contract.Repository.UserManagement;

namespace Glamatek.Contract.Repository.RepositoryManager;
public interface IRepositoryManager
{

    IUserRepository UserRepository { get; }

}
