using Dawem.Models.Dtos.Dawem.SignalR;

namespace Dawem.BusinessLogic.Dawem.RealTime.SignalR
{
    public interface ISignalRHubClient
    {
        public Task ReceiveNewNotification(TempNotificationModelDTO model);
    }
}
