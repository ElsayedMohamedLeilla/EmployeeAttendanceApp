using AutoMapper;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Dtos.Others.VacationBalances;

namespace Dawem.Models.AutoMapper.Others
{
    public class VacationBalancesMapProfile : Profile
    {
        public VacationBalancesMapProfile()
        {
            CreateMap<CreateVacationBalanceModel, VacationBalance>()
                .ForMember(dest => dest.RemainingBalance, opts => opts.MapFrom(src => src.Balance))
                .ForMember(dest => dest.ExpirationDate, opts => opts.MapFrom(src => new DateTime(src.Year, 12, 31)));
            CreateMap<UpdateVacationBalanceModel, VacationBalance>()
                 .ForMember(dest => dest.RemainingBalance, opts => opts.MapFrom(src => src.Balance))
                 .ForMember(dest => dest.ExpirationDate, opts => opts.MapFrom(src => new DateTime(src.Year, 12, 31)));

        }
    }
}
