using AutoMapper;
using Dawem.Domain.Entities.Employees;
using Dawem.Models.Dtos.Employees.TaskType;

namespace Dawem.Models.AutoMapper
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
