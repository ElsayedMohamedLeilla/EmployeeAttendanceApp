﻿namespace Dawem.Models.Response.Dawem.Requests.Vacations
{
    public class EmployeeGetVacationsInformationsResponseDTO
    {
        public float VacationsBalance { get; set; }
        public int AcceptedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
    }
}
