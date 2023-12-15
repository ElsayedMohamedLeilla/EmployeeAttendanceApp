namespace Dawem.Models.Response.Core.Holidaies
{
    public class GetHolidayDropDownResponseDTO
    {
        public List<GetHolidayForDropDownResponseModelDTO> Holidaies { get; set; }
        public int TotalCount { get; set; }
    }
}
