﻿using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        IQueryable<Group> GetAsQueryable(GetGroupCriteria criteria);
    }
}
