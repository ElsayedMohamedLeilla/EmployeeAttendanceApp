using Dawem.Contract.Repository.Others;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Ohters;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Others;
using Dawem.Translations;
using LinqKit;

namespace Dawem.Repository.Others
{
    public class ActionLogRepository : GenericRepository<ActionLog>, IActionLogRepository
    {
        private readonly RequestInfo userContext;
        public ActionLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _userContext) : base(unitOfWork)
        {
            userContext = _userContext;
        }
        public IQueryable<ActionLog> GetAsQueryable(GetActionLogsCriteria criteria, string includeProperties = DawemKeys.EmptyString)
        {
            var outerpredicate = PredicateBuilder.New<ActionLog>(true);

            var inner = PredicateBuilder.New<ActionLog>(true);

            outerpredicate = outerpredicate.And(x => x.BranchId == userContext.BranchId);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.Branch != null && x.Branch.Name.ToLower().Contains(criteria.FreeText));
                inner = inner.Or(x => x.User != null && (x.User.FirstName + x.User.FirstName).ToLower().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    inner = inner.Or(x => x.Id == id);
                }
            }

            if (criteria.Id != null)
            {
                outerpredicate = outerpredicate.And(x => x.Id == criteria.Id.Value);
            }

            if (criteria.Method != null)
            {
                outerpredicate = outerpredicate.And(x => x.ActionType == criteria.Method);
            }

            if (criteria.Screen != null)
            {
                outerpredicate = outerpredicate.And(x => x.ActionPlace == criteria.Screen);
            }

            outerpredicate = outerpredicate.And(inner);
            var query = Get(outerpredicate, includeProperties: includeProperties);
            return query;
        }

    }

}
