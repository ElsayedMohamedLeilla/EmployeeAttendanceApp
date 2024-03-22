using Dawem.Data;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Dtos.Employees.Departments;

namespace Dawem.Contract.Repository.Subscriptions
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        IQueryable<Subscription> GetAsQueryable(GetSubscriptionsCriteria criteria);
    }
}
