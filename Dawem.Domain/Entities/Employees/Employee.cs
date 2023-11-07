using Dawem.Domain.Entities.Provider;
using Dawem.Enums.General;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Employees
{
    [Table(nameof(Employee) + LeillaKeys.S)]
    public class Employee : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
        public int? JobTitleId { get; set; }

        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public string ProfileImageName { get; set; }
        public DateTime JoiningDate { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public int? AnnualVacationBalance { get; set; }
    }
}
