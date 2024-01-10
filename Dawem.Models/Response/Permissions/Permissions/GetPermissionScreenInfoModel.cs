using Dawem.Enums.Permissions;
using Dawem.Models.Response.Permissions.Permissions;

namespace Dawem.Models.Response.Schedules.SchedulePlanLogs
{
    public class GetPermissionScreenInfoModel
    {
        public ApplicationScreenCode ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public List<PermissionScreenActionResponseWithNamesModel> PermissionScreenActions { get; set; }
    }
}
