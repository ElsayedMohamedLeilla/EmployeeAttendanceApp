﻿namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetUserPermissionsResponseModel
    {
        public GetUserPermissionsResponseModel()
        {
            UserPermissions = new List<PermissionScreenResponseWithNamesModel>();
        }
        public bool IsAdmin { get; set; }
        public List<PermissionScreenResponseWithNamesModel> UserPermissions { get; set; }
    }
}
