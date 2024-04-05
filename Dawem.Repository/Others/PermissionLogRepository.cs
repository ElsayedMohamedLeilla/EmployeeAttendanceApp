using Dawem.Contract.Repository.Permissions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Permissions;
using Dawem.Enums.Generals;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Permissions.PermissionLogs;
using LinqKit;

namespace Dawem.Repository.Others
{
    public class PermissionLogRepository : GenericRepository<PermissionLog>, IPermissionLogRepository
    {
        private readonly RequestInfo requestInfo;
        public PermissionLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, RequestInfo _requestInfo) : base(unitOfWork)
        {
            requestInfo = _requestInfo;
        }
        public IQueryable<PermissionLog> GetAsQueryable(GetPermissionLogsCriteria criteria)
        {
            var outerpredicate = PredicateBuilder.New<PermissionLog>(true);

            var inner = PredicateBuilder.New<PermissionLog>(true);

            if (!string.IsNullOrWhiteSpace(criteria.FreeText))
            {
                criteria.FreeText = criteria.FreeText.ToLower().Trim();

                inner = inner.Start(x => x.User != null && x.User.Name.ToLower().Contains(criteria.FreeText));
                if (int.TryParse(criteria.FreeText, out int id))
                {
                    inner = inner.Or(x => x.Id == id);
                }
            }

            if (requestInfo.Type == AuthenticationType.AdminPanel)
            {
                outerpredicate = outerpredicate.And(e => e.CompanyId == null);
            }
            else if (requestInfo.Type == AuthenticationType.DawemAdmin)
            {
                outerpredicate = outerpredicate.And(e => e.CompanyId == requestInfo.CompanyId);
            }

            outerpredicate = outerpredicate.And(e => e.Type == requestInfo.Type);

            if (criteria.Id != null)
            {
                outerpredicate = outerpredicate.And(x => x.Id == criteria.Id.Value);
            }

            if (criteria.UserId != null)
            {
                outerpredicate = outerpredicate.And(x => x.UserId == criteria.UserId.Value);
            }
            if (criteria.ActionCode != null)
            {
                outerpredicate = outerpredicate.And(x => x.ActionCode == criteria.ActionCode);
            }

            if (criteria.ScreenCode != null)
            {
                outerpredicate = outerpredicate.And(x => x.ScreenCode == criteria.ScreenCode);
            }

            outerpredicate = outerpredicate.And(inner);
            var query = Get(outerpredicate);
            return query;
        }

    }

}
