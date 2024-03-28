using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Core.Holidays
{
    public class GetHolidayForGridForEmployeeDTO
    {
        public int Id { get; set; }

        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string DateType { get; set; }
        public string Period { get; set; }
        public HolidayStatus Status { get; set; }
        public string StartStatus { get; set; }    
    }
}
