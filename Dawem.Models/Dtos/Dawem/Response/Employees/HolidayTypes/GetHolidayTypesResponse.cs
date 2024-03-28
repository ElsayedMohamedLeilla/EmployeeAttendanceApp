namespace Dawem.Models.Response.Employees.HolidayTypes
{
    public class GetHolidayTypesResponse
    {
        public List<GetHolidayTypesResponseModel> HolidayTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
