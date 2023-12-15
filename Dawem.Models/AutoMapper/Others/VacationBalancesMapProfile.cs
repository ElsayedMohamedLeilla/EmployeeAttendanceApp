using AutoMapper;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Schedules.SchedulePlans;

namespace Dawem.Models.AutoMapper
{
    public class VacationBalancesMapProfile : Profile
    {
        public VacationBalancesMapProfile()
        {
            CreateMap<CreateVacationBalanceModel, VacationBalance>();
            CreateMap<UpdateVacationBalanceModel, VacationBalance>();

        }
    }
}
