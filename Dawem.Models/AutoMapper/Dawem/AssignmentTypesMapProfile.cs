using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.AssignmentTypes;

namespace Dawem.Models.AutoMapper.Dawem
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
