﻿using Dawem.Contract.Repository.UserManagement;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.UserManagement
{
    public class UserTokenRepository : GenericRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
    }

}
