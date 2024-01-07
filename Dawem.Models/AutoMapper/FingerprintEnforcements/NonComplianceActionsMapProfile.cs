using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.FingerprintEnforcements.NonComplianceActions;

namespace Dawem.Models.AutoMapper
{
    public class NonComplianceActionsMapProfile : Profile
    {
        public NonComplianceActionsMapProfile()
        {
            CreateMap<CreateNonComplianceActionModel, NonComplianceAction>();
            CreateMap<UpdateNonComplianceActionModel, NonComplianceAction>();
        }
    }
}
