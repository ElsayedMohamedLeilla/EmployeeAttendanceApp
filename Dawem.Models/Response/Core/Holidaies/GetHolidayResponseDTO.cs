namespace Dawem.Models.Response.Core.Holidaies
{
    public class GetHolidayResponseForEmployeeDTO
    {
        public List<GetHolidayForGridForEmployeeDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
