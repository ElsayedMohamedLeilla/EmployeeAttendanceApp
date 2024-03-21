using Dawem.Data;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Criteria.Lookups;
using Dawem.Translations;

namespace Dawem.Contract.Repository.Lookups
{
    public interface ILanguageRepository : IGenericRepository<Language>
    {
        IQueryable<Language> GetAsQueryable(GetLanguagesCriteria criteria, string includeProperties = LeillaKeys.EmptyString);
    }
}
