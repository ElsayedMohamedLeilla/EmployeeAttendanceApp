﻿using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupEmployees;
using Dawem.Models.Dtos.Dawem.Employees.Employees.GroupManagarDelegators;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Dawem.Core.Groups
{
    public class CreateGroupDTO
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> EmployeeIds { get; set; }
        [JsonIgnore] // This property will be excluded from JSON serialization
        public List<GroupEmployeeCreateModelDTO> Employees { get; set; }
        public int ManagerId { get; set; }
        public List<int> ManagerDelegatorIds { get; set; }
        [JsonIgnore]
        public List<GroupManagarDelegatorCreateModelDTO> ManagerDelegators { get; set; }
        public List<int> ZoneIds { get; set; }
        [JsonIgnore]
        public List<ZoneGroupCreateModelDTO> Zones { get; set; }
        public void MapGroupEmployees()
        {
            Employees = EmployeeIds
                .Select(employeeId => new GroupEmployeeCreateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapGroupManagarDelegators()
        {
            ManagerDelegators = ManagerDelegatorIds
                .Select(employeeId => new GroupManagarDelegatorCreateModelDTO { EmployeeId = employeeId })
                .ToList();
        }
        public void MapGroupZones()
        {
            Zones = ZoneIds != null ? ZoneIds
                .Select(zoneId => new ZoneGroupCreateModelDTO { ZoneId = zoneId })
                .ToList() : null;
        }
    }
}
