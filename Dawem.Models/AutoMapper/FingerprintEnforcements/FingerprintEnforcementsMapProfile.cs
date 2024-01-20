using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements;

namespace Dawem.Models.AutoMapper
{
    public class FingerprintEnforcementsMapProfile : Profile
    {
        public FingerprintEnforcementsMapProfile()
        {
            CreateMap<CreateFingerprintEnforcementModel, FingerprintEnforcement>()
                 .AfterMap(Maptypes);
            CreateMap<UpdateFingerprintEnforcementModel, FingerprintEnforcement>()
                .AfterMap(Maptypes);
        }
        private void Maptypes(BaseFingerprintEnforcementModel source, FingerprintEnforcement destination, ResolutionContext context)
        {
            destination.FingerprintEnforcementNotifyWays = source.NotifyWays != null ?
               source.NotifyWays.Select(e => new FingerprintEnforcementNotifyWay() { NotifyWay = e }).ToList() : null;
            destination.FingerprintEnforcementEmployees = source.Employees != null ?
               source.Employees.Select(e => new FingerprintEnforcementEmployee() { EmployeeId = e }).ToList() : null;
            destination.FingerprintEnforcementGroups = source.Groups != null ?
               source.Groups.Select(g => new FingerprintEnforcementGroup() { GroupId = g }).ToList() : null;
            destination.FingerprintEnforcementDepartments = source.Departments != null ?
               source.Departments.Select(d => new FingerprintEnforcementDepartment() { DepartmentId = d }).ToList() : null;
            destination.FingerprintEnforcementActions = source.Actions != null ?
               source.Actions.Select(a => new FingerprintEnforcementAction() { NonComplianceActionId = a }).ToList() : null;
        }
    }
}
