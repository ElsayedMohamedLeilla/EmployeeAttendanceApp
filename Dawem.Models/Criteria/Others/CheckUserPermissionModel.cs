﻿using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;

namespace Dawem.Models.Criteria.Others
{
    public class CheckUserPermissionModel : BaseCriteria
    {
        public int UserId { get; set; }
        public int ScreenCode { get; set; }
        public ApplicationActionCode ActionCode { get; set; }
        public string ActionName { get; set; }
        public ApplicationType ApplicationType { get; set; }
    }
}
