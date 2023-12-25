using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Requests.Assignments
{
    public class GetAssignmentDayModel
    {
        public int Day { get; set; }
        public WeekDay WeekDay { get; set; }
        public string WeekDayName { get; set; }
        public List<GetAssignmentModel> Assignments { get; set; }
    }
}
