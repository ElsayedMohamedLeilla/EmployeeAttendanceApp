﻿using Dawem.Contract.Repository.Lookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Lookups
{
    public class OldScreenRepository : GenericRepository<OldNotUsedScreen>, IOldScreenRepository
    {
        public OldScreenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
