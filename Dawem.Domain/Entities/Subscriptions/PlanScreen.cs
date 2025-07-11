﻿using Dawem.Domain.Entities.Others;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Subscriptions
{
    [Table(nameof(PlanScreen) + LeillaKeys.S)]
    public class PlanScreen : BaseEntity
    {
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public Plan Plan { get; set; }
        public int ScreenId { get; set; }
        [ForeignKey(nameof(ScreenId))]
        public MenuItem Screen { get; set; }
    }

}
