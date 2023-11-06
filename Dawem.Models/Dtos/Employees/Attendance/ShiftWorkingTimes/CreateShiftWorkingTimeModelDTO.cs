using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class CreateShiftWorkingTimeModelDTO
    {

        public string Name { get; set; }
        public AmPm TimePeriod { get; set; }
        public TimeOnly CheckInTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public double AllowedMinutes { get; set; }
        public  bool IsActive { get; set; }

    }
}
