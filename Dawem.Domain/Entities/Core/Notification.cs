﻿using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(Notification) + LeillaKeys.S)]
    public class Notification : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public bool IsRead { get; set; }
        public NotificationStatus Status { get; set; }
        public NotificationPriority Priority { get; set; }
        public NotificationType NotificationType { get; set; }
        public DawemAdminApplicationScreenCode ScreenCode { get; set; }
        public string NotificationTypeName { get; set; }
        public bool IsViewed { get; set; }
        public int? HelperNumber { get; set; }
        public DateTime? HelperDate { get; set; }
        public void MarkAsRead()
        {
            IsRead = true;
            ModifiedDate = DateTime.UtcNow;
        }
        public void MarkAsViewed()
        {
            IsViewed = true;
            ModifiedDate = DateTime.UtcNow;
        }
        public virtual List<NotificationTranslation> NotificationTranslations { get; set; }
        public virtual List<NotificationEmployee> NotificationEmployees { get; set; }
    }
}
