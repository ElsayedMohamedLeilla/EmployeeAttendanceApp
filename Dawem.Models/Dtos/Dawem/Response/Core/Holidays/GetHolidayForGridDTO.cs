namespace Dawem.Models.Response.Core.Holidays
{
    public class GetHolidayForGridDTO
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
