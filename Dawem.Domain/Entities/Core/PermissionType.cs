﻿using Dawem.Domain.Entities.Providers;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(PermissionType) + LeillaKeys.S)]
    public class PermissionType : BaseEntity
    {
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }


    }
}
