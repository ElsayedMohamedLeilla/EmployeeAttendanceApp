using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(NotificationStore) + LeillaKeys.S)]
    public class NotificationStore : BaseEntity
    {
        #region Forign Key
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        #endregion
        public int Code { get; set; }
        public string ShortMessege { get; set; }
        public string FullMessege { get; set; }
        public bool IsRead { get; set; }
        public NotificationStatus Status { get; set; }
        public string IconUrl { get; set; }
        public Priority Priority { get; set; }
        public int RecipientUserId { get; set; }

        public void MarkAsRead()
        {
            IsRead = true;
            ModifiedDate = DateTime.UtcNow;
        }




    }
}
