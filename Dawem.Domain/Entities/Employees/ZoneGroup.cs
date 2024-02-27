using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(ZoneGroup) + LeillaKeys.S)]
    public class ZoneGroup : BaseEntity
    {
        #region Foregn Keys
        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        public int ZoneId { get; set; }
        [ForeignKey(nameof(ZoneId))]
        public Zone Zone { get; set; }

        #endregion

    }
}
