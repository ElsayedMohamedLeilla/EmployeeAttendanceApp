﻿using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(Plan) + LeillaKeys.S)]
    public class Plan : BaseEntity
    {
        public int Code { get; set; }
        public int MinNumberOfEmployees { get; set; }
        public int MaxNumberOfEmployees { get; set; }
        public decimal EmployeeCost { get; set; }
        public bool IsTrial { get; set; }
        public bool AllScreensAvailable { get; set; }
        public List<PlanNameTranslation> NameTranslations { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public List<PlanScreen> PlanScreens { get; set; }
    }
}
