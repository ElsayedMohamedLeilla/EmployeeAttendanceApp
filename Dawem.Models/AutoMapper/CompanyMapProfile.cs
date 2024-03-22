using AutoMapper;
using Dawem.Domain.Entities.Providers;
using Dawem.Domain.Entities.Requests;
using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.AutoMapper
{
    public class CompanyMapProfile : Profile
    {
        public CompanyMapProfile()
        {
            CreateMap<CreateCompanyModel, Company>().
                AfterMap(MapCompany);
            CreateMap<UpdateCompanyModel, Company>().
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
