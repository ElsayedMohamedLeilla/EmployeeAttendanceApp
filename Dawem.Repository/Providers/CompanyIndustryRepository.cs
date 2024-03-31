using Dawem.Contract.Repository.Provider;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Providers;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.Providers
{
    public class CompanyIndustryRepository : GenericRepository<CompanyIndustry>, ICompanyIndustryRepository
    {
        public CompanyIndustryRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
