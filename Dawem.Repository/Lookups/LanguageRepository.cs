using Dawem.Contract.Repository.Lookups;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Criteria.Lookups;
using Dawem.Models.DTOs.Dawem.Generic;
using Dawem.Translations;
using LinqKit;

namespace Dawem.Repository.Lookups
{
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
            

        }
        public IQueryable<Language> GetAsQueryable(GetLanguagesCriteria criteria, string includeProperties = LeillaKeys.EmptyString)
        {
            var predicate = PredicateBuilder.New<Language>(true);

            if (criteria.Id > 0)
            {
                predicate = predicate.And(x => x.Id == criteria.Id);
            }

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                predicate = predicate.Start(x => x.Name.ToLower().Trim().StartsWith(criteria.FreeText));
                predicate = predicate.Or(x => x.NativeName.ToLower().Trim().StartsWith(criteria.FreeText));
                predicate = predicate.Or(x => x.ISO2.ToLower().Trim().StartsWith(criteria.FreeText));
                predicate = predicate.Or(x => x.ISO3.ToLower().Trim().StartsWith(criteria.FreeText));
            }

            var query = Get(predicate, includeProperties: includeProperties);
            return query;
        }
    }
}
