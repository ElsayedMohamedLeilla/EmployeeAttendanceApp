using Dawem.Contract.Repository.Subscriptions;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Models.Generic;

namespace Dawem.Repository.Providers
{
    public class SubscriptionLogRepository : GenericRepository<SubscriptionLog>, ISubscriptionLogRepository
    {
        public SubscriptionLogRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
