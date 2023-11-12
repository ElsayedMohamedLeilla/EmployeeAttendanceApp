using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Attendance
{
    [Table(nameof(SchedulePlanDepartment) + LeillaKeys.S)]
    public class SchedulePlanDepartment : BaseEntity
    {
        #region Foregn Keys

        public int SchedulePlanId { get; set; }
        [ForeignKey(nameof(SchedulePlanId))]
        public SchedulePlan SchedulePlan { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }

        #endregion
    }
}
