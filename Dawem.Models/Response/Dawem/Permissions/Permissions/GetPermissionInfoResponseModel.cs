﻿using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionInfoResponseModel
    {
        public int Id { get; set; }
        public ForResponsibilityOrUser ForType { get; set; }
        public string ForTypeName { get; set; }
        public string ResponsibilityName { get; set; }
        public string UserName { get; set; }
        public int Code { get; set; }
        public List<PermissionScreenResponseWithNamesModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
