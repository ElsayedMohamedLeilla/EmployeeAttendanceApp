using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Core.JustificationsTypes
{
    public class JustificationsTypeRepository : GenericRepository<JustificationType>, IJustificationsTypeRepository
    {
        public JustificationsTypeRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }

        public IQueryable<JustificationType> GetAsQueryable(GetJustificationsTypesCriteria criteria)
        {
            var predicate = PredicateBuilder.New<JustificationType>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<JustificationType>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();
                inner = inner.And(x => x.Name.ToLower().Trim().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    criteria.Id = id;
                }
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
