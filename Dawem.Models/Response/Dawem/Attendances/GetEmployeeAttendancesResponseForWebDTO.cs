﻿namespace Dawem.Models.Response.Dawem.Attendances
{
    public class GetEmployeeAttendancesResponseForWebDTO
    {
        public List<GetEmployeeAttendancesResponseForWebAdminModelDTO> EmployeeAttendances { get; set; }
        public int TotalCount { get; set; }


    }
}
