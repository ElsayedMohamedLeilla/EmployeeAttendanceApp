﻿using Dawem.Models.Dtos.Dawem.Subscriptions.Plans;

namespace Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions
{
    public interface IPlanBLValidation
    {
        Task<bool> CreateValidation(CreatePlanModel model);
        Task<bool> UpdateValidation(UpdatePlanModel model);
    }
}
