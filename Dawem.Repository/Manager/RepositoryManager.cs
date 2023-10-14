using Dawem.Contract.Repository.Manager;
using Dawem.Contract.Repository.Others;
using Dawem.Contract.Repository.Provider;
using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Repository.Others;
using Dawem.Repository.Provider;
using Dawem.Repository.UserManagement;

namespace Dawem.Repository.Manager
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly GeneralSetting generalSetting;
        private readonly RequestHeaderContext requestHeaderContext;
        
        
        private IUserRepository userRepository;
        private IActionLogRepository actionLogRepository;
        private IUserBranchRepository userBranchRepository;

        public RepositoryManager(IUnitOfWork<ApplicationDBContext> _unitOfWork, GeneralSetting _generalSetting, RequestHeaderContext _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            generalSetting = _generalSetting;
            requestHeaderContext = _requestHeaderContext;

        }

        public IUserRepository UserRepository =>
         userRepository ??= new UserRepository(requestHeaderContext, unitOfWork, generalSetting);

        public IActionLogRepository ActionLogRepository =>
         actionLogRepository ??= new ActionLogRepository(unitOfWork, requestHeaderContext);

        public IUserBranchRepository UserBranchRepository =>
        userBranchRepository ??= new UserBranchRepository(unitOfWork, generalSetting);

        
    }
}
