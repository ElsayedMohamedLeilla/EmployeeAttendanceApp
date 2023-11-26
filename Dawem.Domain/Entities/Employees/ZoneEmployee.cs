using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(ZoneEmployee) + LeillaKeys.S)]
    public class ZoneEmployee : BaseEntity
    {
        #region Foregn Keys
        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public int ZoneId { get; set; }
        [ForeignKey(nameof(ZoneId))]
        public Zone Zone { get; set; }

        #endregion

    }
}
