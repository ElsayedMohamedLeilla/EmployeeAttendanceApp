using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendance
{
    [Table(nameof(SchedulePlanEmployee) + LeillaKeys.S)]
    public class SchedulePlanEmployee : BaseEntity
    {
        #region Foregn Keys

        public int SchedulePlanId { get; set; }
        [ForeignKey(nameof(SchedulePlanId))]
        public SchedulePlan SchedulePlan { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        #endregion
    }
}
