using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Provider;
using Dawem.Models.Generic;
using Dawem.Repository.Provider.Contract;

namespace Dawem.Repository.Provider
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
