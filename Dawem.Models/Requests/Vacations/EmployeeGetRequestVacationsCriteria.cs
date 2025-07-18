﻿using Dawem.Enums.Generals;
using Dawem.Models.Criteria;

namespace Dawem.Models.Requests.Vacations
{
    public class EmployeeGetRequestVacationsCriteria : BaseCriteria
    {
        public int? VacationTypeId { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? Date { get; set; }
        public VacationStatus VacationStatus { get; set; }
    }
}
