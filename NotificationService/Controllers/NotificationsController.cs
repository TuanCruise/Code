using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        //public override Task OnConnectedAsync()
        //{
        //    Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} joined the conversation");
        //    return base.OnConnectedAsync();
        //}
        //public void Send(string name, string message)
        //{
        //    Clients.All.SendAsync("broadcastMessage", name, message);
        //}

        //public override Task OnDisconnectedAsync(System.Exception exception)
        //{
        //    Clients.All.SendAsync("broadcastMessage", "system", $"{Context.ConnectionId} left the conversation");
        //    return base.OnDisconnectedAsync(exception);
        //}
    }
}