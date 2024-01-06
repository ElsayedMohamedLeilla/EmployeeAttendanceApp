using Dawem.Models.Dtos.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Dawem.BusinessLogic.SignalR
{
    // [Authorize]
    public class SignalRHub : Hub<ISignalRHubClient>
    {
        public SignalRHub()
        {
        }
        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string method, string groupName, SignalRMessageModelDTO model)
        {
            switch (method.ToLower())
            {
                case "receivevacationrequest":
                    await Clients.Group(groupName).NewVacationRequest(model);
                    break;
                default:
                    break;
            }
        }





    }
}
