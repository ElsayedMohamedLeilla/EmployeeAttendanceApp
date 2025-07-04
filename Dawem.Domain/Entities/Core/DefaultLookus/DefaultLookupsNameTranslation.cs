﻿using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Schedules;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core.DefaultLookus
{
    [Table(nameof(DefaultLookupsNameTranslation) + LeillaKeys.S)]
    public class DefaultLookupsNameTranslation : NameTranslation
    {
        public int DefaultLookupId { get; set; }
        [ForeignKey(nameof(DefaultLookupId))]
        public DefaultLookup DefaultLookup { get; set; }

    }
}
