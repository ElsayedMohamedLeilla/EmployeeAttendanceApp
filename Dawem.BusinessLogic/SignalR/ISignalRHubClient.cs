using Dawem.Models.Dtos.SignalR;

namespace Dawem.BusinessLogic.SignalR
{
    public interface ISignalRHubClient
    {
        public Task ReceiveNewNotification(TempNotificationModelDTO model); 
    }
}
