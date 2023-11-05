using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Employees.Employees
{
    public class UpdateShiftWorkingTimeModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AmPm TimePeriod { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public double AllowedMinutes { get; set; }
        public bool IsFreezed { get; set; }
        public bool IsActive { get; set; }


    }
}
