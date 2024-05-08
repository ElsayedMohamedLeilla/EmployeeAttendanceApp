using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
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
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }
        #endregion
        public string ShortMessege { get; set; }
        public string FullMessage { get; set; }
        public bool IsRead { get; set; }
        public NotificationStatus Status { get; set; }
        public Priority Priority { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsViewed { get; set; }
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
    }
}
