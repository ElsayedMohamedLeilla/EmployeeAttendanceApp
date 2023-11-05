using Dawem.Data;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Criteria.Lookups;
using Dawem.Translations;

namespace Dawem.Contract.Repository.Lookups
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        IQueryable<Currency> GetAsQueryable(GetCurrenciesCriteria criteria, string includeProperties = LeillaKeys.EmptyString);
    }
}
