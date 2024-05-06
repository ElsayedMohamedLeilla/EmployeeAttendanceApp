using Dawem.Models.Criteria.Core;

namespace Dawem.Contract.BusinessLogic.Dawem.Core
{
    public interface INotificationHandleBL
    {
        Task<bool> HandleNotifications(HandleNotificationModel model);
    }
}
