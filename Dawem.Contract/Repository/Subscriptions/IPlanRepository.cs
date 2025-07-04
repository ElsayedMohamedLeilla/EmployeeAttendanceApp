﻿using Dawem.Data;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;

namespace Dawem.Contract.Repository.Subscriptions
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        IQueryable<Plan> GetAsQueryable(GetPlansCriteria criteria);
    }
}
