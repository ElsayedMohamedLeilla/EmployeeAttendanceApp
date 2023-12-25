namespace Dawem.Models.Response.Core.Holidays
{
    public class GetHolidayDropDownResponseDTO
    {
        public List<GetHolidayForDropDownResponseModelDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
