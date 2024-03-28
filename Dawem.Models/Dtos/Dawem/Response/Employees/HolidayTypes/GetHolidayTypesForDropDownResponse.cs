namespace Dawem.Models.Response.Employees.HolidayTypes
{
    public class GetHolidayTypesForDropDownResponse
    {
        public List<GetHolidayTypesForDropDownResponseModel> HolidayTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
