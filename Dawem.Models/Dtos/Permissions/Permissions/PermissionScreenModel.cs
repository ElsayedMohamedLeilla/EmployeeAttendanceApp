using Dawem.Enums.Configration;

namespace Dawem.Models.Dtos.Employees.AssignmentTypes
{
    public class PermissionScreenModel
    {
        public ApplicationScreenCode ScreenCode { get; set; }
        public List<PermissionScreenActionModel> PermissionScreenActions { get; set; }
    }
}
