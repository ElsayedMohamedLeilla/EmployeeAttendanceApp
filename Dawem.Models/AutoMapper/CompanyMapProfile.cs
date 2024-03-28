using AutoMapper;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.Dtos.Providers.Companies;

namespace Dawem.Models.AutoMapper
{
    public class CompanyMapProfile : Profile
    {
        public CompanyMapProfile()
        {
            CreateMap<CreateCompanyModel, Company>().
                ForMember(dest => dest.CompanyIndustries, opt => opt.MapFrom(src => src.Industries)).
                ForMember(dest => dest.CompanyBranches, opt => opt.MapFrom(src => src.Branches)).
                AfterMap(MapCompany);

            CreateMap<UpdateCompanyModel, Company>().
                ForMember(dest => dest.CompanyIndustries, opt => opt.MapFrom(src => src.Industries)).
                ForMember(dest => dest.CompanyBranches, opt => opt.MapFrom(src => src.Branches)).
                AfterMap(MapCompany);

            CreateMap<CompanyIndustryModel, CompanyIndustry>();
            CreateMap<CompanyBranchModel, CompanyBranch>();

        }

        private void MapCompany(CreateCompanyModel source, Company destination, ResolutionContext context)
        {
            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.CompanyAttachments = source.AttachmentsNames
                .Select(fileName => new CompanyAttachment { FileName = fileName })
                .ToList();
            }
        }
        private void MapCompany(UpdateCompanyModel source, Company destination, ResolutionContext context)
        {
            if (source.AttachmentsNames != null && source.AttachmentsNames.Count > 0)
            {
                destination.CompanyAttachments = source.AttachmentsNames
                .Select(fileName => new CompanyAttachment { FileName = fileName })
                .ToList();
            }
        }
    }
}
