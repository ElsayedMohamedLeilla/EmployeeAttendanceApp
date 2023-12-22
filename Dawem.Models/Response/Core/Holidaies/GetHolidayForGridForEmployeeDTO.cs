namespace Dawem.Models.Response.Core.Holidaies
{
    public class GetHolidayForGridForEmployeeDTO
    {
        public int Id { get; set; }

        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string DateType { get; set; }
        public string Notes { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
