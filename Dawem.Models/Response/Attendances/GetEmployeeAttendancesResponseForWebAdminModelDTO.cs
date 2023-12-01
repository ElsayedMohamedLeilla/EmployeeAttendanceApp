using Dawem.Domain.Entities.Employees;
using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Attendances
{
    public class GetEmployeeAttendancesResponseForWebAdminModelDTO
    {
        public int id { get; set; } //employeeAteendance Id
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateOnly Date { get; set; } // ateendance Date
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public string Status { get; set; }
        public double TimeGap { get; set; }
        public string WayOfRecognition { get; set; }
        //public double Latitude { get; set; }
        //public double Longitude { get; set; }

        public string ZoneName { get; set; }





    }
}
