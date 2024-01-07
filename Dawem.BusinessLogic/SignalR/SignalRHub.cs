using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Dawem.BusinessLogic.SignalR
{
     [Authorize]
    public class SignalRHub : Hub<ISignalRHubClient>
    {
        public SignalRHub()
        {
        }
        public Task JoinGroup(string groupName)
        {
            Console.Write("Connected To " + groupName);
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }


    }
}
