using Dawem.Contract.RealTime.Firebase;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.RealTime.Firebase;
using Dawem.Models.Generic;

namespace Dawem.Repository.RealTime.Firebase
{
    public class NotificationUserDeviceTokenRepository : GenericRepository<NotificationUserDeviceToken>, INotificationUserDeviceTokenRepository
    {
        public NotificationUserDeviceTokenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
