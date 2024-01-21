using Dawem.Contract.Repository.Employees;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Firebase;
using Dawem.Models.Generic;

namespace Dawem.Repository.Employees
{
    public class NotificationUserDeviceTokenRepository : GenericRepository<NotificationUserDeviceToken>, INotificationUserDeviceTokenRepository
    {
        public NotificationUserDeviceTokenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
