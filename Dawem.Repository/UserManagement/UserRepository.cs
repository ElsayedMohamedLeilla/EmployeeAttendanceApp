using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.Context;
using Dawem.Models.Generic;

namespace Dawem.Repository.UserManagement
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly RequestHeaderContext requestHeaderContext;
        public UserRepository(RequestHeaderContext _requestHeaderContext, IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            requestHeaderContext = _requestHeaderContext;
        }
       
    }

}
