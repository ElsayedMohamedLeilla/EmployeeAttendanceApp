﻿namespace Dawem.Models.Response.Dawem.Core.PermissionsTypes
{
    public class GetPermissionsTypeDropDownResponseDTO
    {
        public List<GetPermissionsTypeForDropDownResponseModelDTO> PermissionsTypes { get; set; }
        public int TotalCount { get; set; }
    }
}
