namespace Dawem.Models.Response.Core.Holidaies
{
    public class GetHolidayResponseForEmployeeDTO
    {
        public List<GetHolidayForGridForEmployeeDTO> Holidays { get; set; }
        public int TotalCount { get; set; }
    }
}
