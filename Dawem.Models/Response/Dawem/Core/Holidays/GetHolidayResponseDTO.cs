namespace Dawem.Models.Response.Dawem.Core.Holidays
{
    public class GetHolidayResponseForEmployeeDTO
    {
        public List<GetHolidayForGridForEmployeeDTO> Holidays { get; set; }
        public int TotalCount { get; set; }
    }
}
