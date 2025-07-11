﻿using Dawem.Domain.Entities.Employees;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Core
{
    [Table(nameof(GroupManagerDelegator) + LeillaKeys.S)]
    public class GroupManagerDelegator : BaseEntity
    {
        #region Foregn Keys
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        #endregion

    }
}
