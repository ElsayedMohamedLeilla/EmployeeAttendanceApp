﻿namespace Dawem.Models.Response.Dawem.Requests.Permissions
{
    public class GetPermissionsInformationsResponseDTO
    {
        public int TotalPermissionsCount { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
