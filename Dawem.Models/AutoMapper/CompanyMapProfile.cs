using AutoMapper;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper
{
    public class CompanyMapProfile : Profile
    {
        public CompanyMapProfile()
        {
            CreateMap<CreateCompanyModel, Company>()
                .ForMember(dest => dest.CompanyIndustries, 
                opt => opt.MapFrom(src => src.Industries.Select(i => new CompanyIndustry { Name = i })));
            CreateMap<UpdateCompanyModel, Company>()
                .ForMember(dest => dest.CompanyIndustries, 
                opt => opt.MapFrom(src => src.Industries.Select(i => new CompanyIndustry { Name = i })));

        }
    }
}
