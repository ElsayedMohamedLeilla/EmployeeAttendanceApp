using Dawem.Domain.Entities.Attendances;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Others;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Requests;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.Summons;
using Dawem.Domain.Entities.UserManagement;
using Dawem.Enums.Generals;
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
        public int? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
        public int? JobTitleId { get; set; }
        [ForeignKey(nameof(JobTitleId))]
        public JobTitle JobTitle { get; set; }
        public int? ScheduleId { get; set; }
        [ForeignKey(nameof(ScheduleId))]
        public Schedule Schedule { get; set; }
        public int? DirectManagerId { get; set; }
        [ForeignKey(nameof(DirectManagerId))]
        public Employee DirectManager { get; set; }
        public int MobileCountryId { get; set; }
        [ForeignKey(nameof(MobileCountryId))]
        public Country MobileCountry { get; set; }

        #endregion
        public int Code { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string ProfileImageName { get; set; }
        public string FingerprintMobileCode { get; set; }
        public bool AllowChangeFingerprintMobileCode { get; set; }
        public bool AllowFingerprintOutsideAllowedZones { get; set; }
        public bool InsertedFromExcel { get; set; }
        public DateTime JoiningDate { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public List<GroupEmployee> EmployeeGroups { get; set; }
        public List<ZoneEmployee> Zones { get; set; }
        public List<VacationBalance> VacationBalances { get; set; }
        public List<EmployeeAttendance> EmployeeAttendances { get; set; }
        public List<Request> EmployeeRequests { get; set; }
        public List<RequestTaskEmployee> EmployeeTasks { get; set; }
        public List<MyUser> Users { get; set; }
        public List<SchedulePlanEmployee> SchedulePlanEmployees { get; set; }
        public int? FingerprintDeviceUserCode { get; set; }
        public List<SummonLog> SummonLogs { get; set; }
        public virtual List<NotificationEmployee> NotificationEmployees { get; set; }

        //#region New Proos

        //public Gender Gender { get; set; }
        //public DateTime? BirthDate { get; set; }
        //public string Nationality { get; set; }
        //public string MaritalStatus { get; set; }
        //public string EmergencyContact { get; set; }
        //public string BranchName { get; set; }
        //public int? NumberOfYearsOfExperience { get; set; }
        //public string CostCenterName { get; set; }
        //public string NationalIdNumber { get; set; }
        //public string PassportNumber { get; set; }
        //public DateTime? PassportEndDate { get; set; }
        //public DateTime? ResidenceEndDate { get; set; }

        //#region Employee medical information
        //public string BloodType { get; set; }
        //public string MedicalInsuranceNumber { get; set; }
        //public string DiseasesAndMedicalConditions { get; set; }

        //#endregion

        //#region Salary information
        //public string BankAccountNumber { get; set; }
        //public string BankAccountIBANNumber { get; set; }
        //public decimal? TotalSalary { get; set; }              

        //#endregion


        //#endregion
    }
}
