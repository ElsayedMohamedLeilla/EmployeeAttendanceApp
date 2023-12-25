namespace Dawem.Models.Response.Core.Holidays
{
    public class GetHolidayResponseDTO
    {
        public List<GetHolidayForGridDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
