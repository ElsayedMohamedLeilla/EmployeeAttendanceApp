using AutoMapper;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Employees.Departments;

namespace Dawem.Models.AutoMapper
{
    public class PlansMapProfile : Profile
    {
        public PlansMapProfile()
        {
            CreateMap<CreatePlanModel, Plan>();
            CreateMap<UpdatePlanModel, Plan>();

        }
    }
}
