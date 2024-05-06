using Dawem.Data;
using Dawem.Domain.Entities.Core;
using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.Repository.Core
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        IQueryable<Notification> GetAsQueryable(GetNotificationCriteria criteria);
    }
}
