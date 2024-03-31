using Dawem.Contract.RealTime.Firebase;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.RealTime.Firebase;
using Dawem.Models.DTOs.Dawem.Generic;

namespace Dawem.Repository.RealTime.Firebase
{
    public class NotificationUserFCMTokenRepository : GenericRepository<NotificationUserFCMToken>, INotificationUserFCMTokenRepository
    {
        public NotificationUserFCMTokenRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
