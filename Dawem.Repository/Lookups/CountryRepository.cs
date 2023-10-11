using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Generic;
using Dawem.Repository.Lookups.Contract;

namespace Dawem.Repository.Lookups
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
