namespace Dawem.Models.Response.Employees.HolidayTypes
{
    public class GetHolidayTypesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
