using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Dawem.Employees.TaskTypes;

namespace Dawem.Models.AutoMapper.Dawem
{
    public class TaskTypesMapProfile : Profile
    {
        public TaskTypesMapProfile()
        {
            CreateMap<CreateTaskTypeModel, TaskType>();
            CreateMap<UpdateTaskTypeModel, TaskType>();
        }
    }
}
