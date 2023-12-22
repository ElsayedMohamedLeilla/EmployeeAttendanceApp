namespace Dawem.Models.Response.Core.Holidaies
{
    public class GetHolidayResponseDTO
    {
        public List<GetHolidayForGridDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
