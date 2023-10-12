using Dawem.Data;
using Dawem.Domain.Entities.Lookups;
using Dawem.Translations;
using SmartBusinessERP.Models.Criteria.Lookups;

namespace Dawem.Contract.Repository.Lookups
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        IQueryable<Currency> GetAsQueryable(GetCurrenciesCriteria criteria, string includeProperties = DawemKeys.EmptyString);
    }
}
