using Dawem.Domain.Entities.Attendance;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

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
        [ForeignKey(nameof(JobTitleId))]
        public JobTitle JobTitle { get; set; }
        public int? ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }
        #endregion
        public int Code { get; set; }
        public string Name { get; set; }
        public string ProfileImageName { get; set; }
        public DateTime JoiningDate { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public int? AnnualVacationBalance { get; set; }

        //public int GroupId { get; set; }
        //[ForeignKey(nameof(GroupId))]
        //public Group? Group { get; set; }
    }
}
