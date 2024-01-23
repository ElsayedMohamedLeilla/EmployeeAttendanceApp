using Dawem.Contract.Repository.Core;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;
using Dawem.Models.Generic;
using LinqKit;

namespace Dawem.Repository.Core.NotificationsStores
{
    public class NotificationStoreRepository : GenericRepository<NotificationStore>, INotificationStoreRepository
    {
        public NotificationStoreRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {

        }
        public IQueryable<NotificationStore> GetAsQueryable(GetNotificationStoreCriteria criteria)
        {
            var predicate = PredicateBuilder.New<NotificationStore>(a => !a.IsDeleted);
            var inner = PredicateBuilder.New<NotificationStore>(true);

           
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
            if (criteria.EmployeeID != null)
            {
                if(criteria.IsRead != null && criteria.IsRead == false)
                {
                    predicate = predicate.And(e => e.EmployeeId == criteria.EmployeeID && e.IsRead == criteria.IsRead);
                }
                else
                {
                    predicate = predicate.And(e => e.EmployeeId == criteria.EmployeeID);
                }
            }

            predicate = predicate.And(inner);
            var Query = Get(predicate);
            return Query;

        }

    }
}
