﻿using Dawem.Enums.Generals;

namespace Dawem.Models.Response.AdminPanel.Subscriptions
{
    public class CheckCompanySubscriptionResponseModel
    {
        public CheckCompanySubscriptionResponseModel()
        {
            Result = true;
        }
        public bool Result { get; set; }
        public CheckCompanySubscriptionErrorType ErrorType { get; set; }
    }
}
