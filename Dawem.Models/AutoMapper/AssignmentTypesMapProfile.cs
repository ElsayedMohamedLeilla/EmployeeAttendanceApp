using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.AssignmentType;

namespace Dawem.Models.AutoMapper
{
    public class AssignmentTypesMapProfile : Profile
    {
        public AssignmentTypesMapProfile()
        {
            CreateMap<CreateAssignmentTypeModel, AssignmentType>();
            CreateMap<UpdateAssignmentTypeModel, AssignmentType>();
        }
    }
}
