using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Dawem.Core.Holidays
{
    public class GetHolidayByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateType DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }





    }
}
