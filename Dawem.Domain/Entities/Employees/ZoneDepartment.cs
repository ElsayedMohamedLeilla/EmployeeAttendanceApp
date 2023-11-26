using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(ZoneDepartment) + LeillaKeys.S)]
    public class ZoneDepartment : BaseEntity
    {
        #region Foregn Keys
        public int? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        public int ZoneId { get; set; }
        [ForeignKey(nameof(ZoneId))]
        public Zone Zone { get; set; }

        #endregion

    }
}
