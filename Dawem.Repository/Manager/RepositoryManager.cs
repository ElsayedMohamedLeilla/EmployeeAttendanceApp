using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Models.Context;
using Dawem.Models.Generic;
using Dawem.Repository.UserManagement;
using Glamatek.Contract.Repository.RepositoryManager;

namespace Glamatek.Repository.Revamp_PhaseOne.RepositoryManager
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly GeneralSetting generalSetting;
        private readonly RequestHeaderContext requestHeaderContext;

        private IUserRepository userRepository;

        public RepositoryManager(IUnitOfWork<ApplicationDBContext> _unitOfWork, GeneralSetting _generalSetting, RequestHeaderContext _requestHeaderContext)
        {
            unitOfWork = _unitOfWork;
            generalSetting = _generalSetting;
            requestHeaderContext = _requestHeaderContext;

        }

        public IUserRepository UserRepository =>
         userRepository ??= new UserRepository(requestHeaderContext, unitOfWork, generalSetting);


    }
}
