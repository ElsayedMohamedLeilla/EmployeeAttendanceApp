﻿using Dawem.Enums.Permissions;

namespace Dawem.Models.Dtos.Dawem.Permissions.Permissions
{
    public class BasePermissionModel
    {
        public ForResponsibilityOrUser ForType { get; set; }
        public int? ResponsibilityId { get; set; }
        public int? UserId { get; set; }
        public List<PermissionScreenModel> Screens { get; set; }
        public bool IsActive { get; set; }
    }
}
