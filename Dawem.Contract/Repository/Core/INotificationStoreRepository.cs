using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface INotificationStoreRepository : IGenericRepository<NotificationStore>
    {
        IQueryable<NotificationStore> GetAsQueryable(GetNotificationStoreCriteria criteria);
    }
}
