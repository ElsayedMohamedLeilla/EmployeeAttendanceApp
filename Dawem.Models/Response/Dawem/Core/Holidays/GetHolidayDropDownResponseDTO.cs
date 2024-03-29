namespace Dawem.Models.Response.Dawem.Core.Holidays
{
    public class GetHolidayDropDownResponseDTO
    {
        public List<GetHolidayForDropDownResponseModelDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
