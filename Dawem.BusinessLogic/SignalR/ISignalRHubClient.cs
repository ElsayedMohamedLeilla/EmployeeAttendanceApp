using Dawem.Models.Dtos.SignalR;

namespace Dawem.BusinessLogic.SignalR
{
    public interface ISignalRHubClient
    {
        public Task SendMessageToGroup( string method,string groupName,SignalRMessageModelDTO model);

        Task NewVacationRequest(SignalRMessageModelDTO model);


    }
}
