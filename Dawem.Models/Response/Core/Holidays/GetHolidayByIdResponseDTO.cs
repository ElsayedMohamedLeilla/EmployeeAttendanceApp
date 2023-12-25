using Dawem.Enums.Generals;
using NodaTime;

namespace Dawem.Models.Response.Core.Holidays
{
    public class GetHolidayByIdResponseDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateType DateType { get; set; }
        public LocalDate StartDate { get; set; }
        public LocalDate EndDate { get; set; }
        public string Notes { get; set; }





    }
}
