using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Others;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;
using LinqKit;

namespace Dawem.Repository.Others
{
    public class ScreenPermissionLogRepository : GenericRepository<ScreenPermissionLog>, IScreenPermissionLogRepository
    {
        private readonly RequestInfo requestInfo;
        public ScreenPermissionLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _requestInfo) : base(unitOfWork)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<ScreenPermissionLog> GetAsQueryable(GetScreenPermissionLogsCriteria criteria, string includeProperties = LeillaKeys.EmptyString)
        {
            var outerpredicate = PredicateBuilder.New<ScreenPermissionLog>(true);

            var inner = PredicateBuilder.New<ScreenPermissionLog>(true);

            outerpredicate = outerpredicate.And(x => x.CompanyId == requestInfo.CompanyId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Company != null && x.Company.Name.ToLower().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    inner = inner.Or(x => x.Id == id);
                }
            }

            if (criteria.Id != null)
            {
                outerpredicate = outerpredicate.And(x => x.Id == criteria.Id.Value);
            }

            if (criteria.ActionType != null)
            {
                outerpredicate = outerpredicate.And(x => x.ActionType == criteria.ActionType);
            }

            if (criteria.ScreenCode != null)
            {
                outerpredicate = outerpredicate.And(x => x.ScreenCode == criteria.ScreenCode);
            }

            outerpredicate = outerpredicate.And(inner);
            var query = Get(outerpredicate, includeProperties: includeProperties);
            return query;
        }

    }

}
