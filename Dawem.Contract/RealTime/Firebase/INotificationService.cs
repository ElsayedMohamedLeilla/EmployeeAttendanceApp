﻿using Dawem.Models.DTOs.Dawem.RealTime.Firebase;

namespace Dawem.Contract.RealTime.Firebase
{
    public interface INotificationService
    {
        Task<ResponseModel> SendNotificationsAndEmails(SendNotificationsAndEmailsModel model);
    }
}
