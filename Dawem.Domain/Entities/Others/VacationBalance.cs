﻿using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Others
{
    [Table(nameof(VacationBalance) + LeillaKeys.S)]
    public class VacationBalance : BaseEntity
    {
        #region Foregn Keys

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        #endregion
        public int Code { get; set; }
        public int Year { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DefaultVacationType DefaultVacationType { get; set; }
        public float Balance { get; set; }
        public float RemainingBalance { get; set; }
    }
}
