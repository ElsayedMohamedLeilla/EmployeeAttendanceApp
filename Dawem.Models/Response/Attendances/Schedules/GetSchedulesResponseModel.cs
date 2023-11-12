namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
    }
}
