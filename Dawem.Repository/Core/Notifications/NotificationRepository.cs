using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Context;
using Dawem.Models.Criteria.Core;
using Dawem.Models.DTOs.Dawem.Generic;
using LinqKit;

namespace Dawem.Repository.Core.Notifications
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly RequestInfo _requestInfo;
        public NotificationRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting, RequestInfo requestInfo) : base(unitOfWork, _generalSetting)
        {
            _requestInfo = requestInfo;
        }
        public IQueryable<Notification> GetAsQueryable(GetNotificationCriteria criteria)
        {
            var predicate = PredicateBuilder.New<Notification>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<Notification>(true);

            predicate = predicate.And(e => e.CompanyId == _requestInfo.CompanyId);

            if (criteria.Id != null)
            {
                predicate = predicate.And(e => e.Id == criteria.Id);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.IsActive != null)
            {
                predicate = predicate.And(e => e.IsActive == criteria.IsActive);
            }
            if (criteria.Ids != null && criteria.Ids.Count > 0)
            {
                predicate = predicate.And(e => criteria.Ids.Contains(e.Id));
            }
            if (criteria.Code != null)
            {
                predicate = predicate.And(ps => ps.Code == criteria.Code);
            }
            if (criteria.EmployeeId > 0)
            {
                predicate = predicate.And(e => e.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.IsRead.HasValue)
            {
                predicate = predicate.And(e => e.IsRead == criteria.IsRead);
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
