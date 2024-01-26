using Dawem.Models.Dtos.SignalR;

namespace Dawem.BusinessLogic.RealTime.SignalR
{
    public interface ISignalRHubClient
    {
        public Task ReceiveNewNotification(TempNotificationModelDTO model);
    }
}
