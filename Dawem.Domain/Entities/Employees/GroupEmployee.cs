using Dawem.Domain.Entities.Core;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(GroupEmployee) + LeillaKeys.S)]
    public class GroupEmployee : BaseEntity
    {
        #region Foregn Keys
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        #endregion

    }
}
