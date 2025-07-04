﻿using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Others
{
    public class UserPermissionsMenuItemWithAvailableActionsDTO
    {
        public int Id { get; set; }
        public GroupOrScreenType GroupOrScreenType { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public List<ApplicationActionCode> AvailableActions { get; set; }
        public List<UserPermissionsMenuItemWithAvailableActionsDTO> Children { get; set; }
    }
}
