using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Providers
{
    [Table(nameof(CompanyAttachment) + LeillaKeys.S)]
    public class CompanyAttachment : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        #endregion

        public string FileName { get; set; }
    }
}
