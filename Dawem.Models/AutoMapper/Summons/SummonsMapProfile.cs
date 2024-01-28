using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Dtos.Summons.Summons;

namespace Dawem.Models.AutoMapper.Summons
{
    public class SummonsMapProfile : Profile
    {
        public SummonsMapProfile()
        {
            CreateMap<CreateSummonModel, Summon>()
                 .AfterMap(Maptypes);
            CreateMap<UpdateSummonModel, Summon>()
                .AfterMap(Maptypes);
        }
        private void Maptypes(BaseSummonModel source, Summon destination, ResolutionContext context)
        {
            destination.SummonNotifyWays = source.NotifyWays != null ?
               source.NotifyWays.Select(e => new SummonNotifyWay() { NotifyWay = e }).ToList() : null;
            destination.SummonEmployees = source.Employees != null ?
               source.Employees.Select(e => new SummonEmployee() { EmployeeId = e }).ToList() : null;
            destination.SummonGroups = source.Groups != null ?
               source.Groups.Select(g => new SummonGroup() { GroupId = g }).ToList() : null;
            destination.SummonDepartments = source.Departments != null ?
               source.Departments.Select(d => new SummonDepartment() { DepartmentId = d }).ToList() : null;
            destination.SummonSanctions = source.Sanctions != null ?
               source.Sanctions.Select(a => new SummonSanction() { SanctionId = a }).ToList() : null;
        }
    }
}
