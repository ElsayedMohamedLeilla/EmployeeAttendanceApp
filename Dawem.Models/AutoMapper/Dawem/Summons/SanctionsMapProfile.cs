using AutoMapper;
using Dawem.Domain.Entities.Summons;
using Dawem.Models.Dtos.Dawem.Summons.Sanctions;

namespace Dawem.Models.AutoMapper.Dawem.Summons
{
    public class SanctionsMapProfile : Profile
    {
        public SanctionsMapProfile()
        {
            CreateMap<CreateSanctionModel, Sanction>();
            CreateMap<UpdateSanctionModel, Sanction>();
        }
    }
}
