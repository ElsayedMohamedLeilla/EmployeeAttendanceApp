namespace Dawem.Models.Response.Dawem.Core.Holidays
{
    public class GetHolidayResponseDTO
    {
        public List<GetHolidayForGridDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
