using Dawem.Contract.Repository.Firebase;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Domain.Entities.Firebase;
using Dawem.Models.Generic;

namespace Dawem.Repository.Firebase
{
    public class NotificationUserRepository : GenericRepository<NotificationUser>, INotificationUserRepository
    {
        public NotificationUserRepository(IUnitOfWork<ApplicationDBContext> unitOfWork, GeneralSetting _generalSetting) : base(unitOfWork, _generalSetting)
        {
        }
    }
}
