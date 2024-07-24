using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TotalHRInsight.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(int orderId, string message)
        {
            // Crea un mensaje formateado con el ID del pedido
            string formattedMessage = $"Pedido #{orderId}: {message}";
            await Clients.All.SendAsync("ReceiveNotification", formattedMessage);
        }
    }
}
