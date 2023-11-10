namespace Dawem.Models.Response.Employees.Attendances.ShiftWorkingTimes
{
    public class GetShiftWorkingTimeDropDownResponseDTO
    {
        public List<GetShiftWorkingTimeForDropDownResponseModelDTO> ShiftWorkingTimes { get; set; }
        public int TotalCount { get; set; }
    }
}
