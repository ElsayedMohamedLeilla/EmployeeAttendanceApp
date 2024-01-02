using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;



namespace Dawem.BusinessLogic.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(int user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message); //ReceiveMessage method that the front will call it 
        }
        public async Task NotifyVacationRequestToManager(int managerId, string  employeeName, DateTime startDate, DateTime endDate)
        {
            await Clients.User(managerId.ToString()).SendAsync("ReceiveVacationRequest", employeeName, startDate,endDate); 
        }

    }
}
