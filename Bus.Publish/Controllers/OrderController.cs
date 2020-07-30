using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WebCore.Entities.Entities;

namespace EBus.Publish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private IBusControl _bus;

        public OrderController(IBusControl bus)
        {
            _bus = bus;
        }
        [HttpGet]
        public string Get()
        {
            return "Welcome to Bus.Masstransit";
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Orders order)
        {
            Uri uri = new Uri("rabbitmq://localhost/order_queue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(order);
            return Ok("success");
        }
    }
}
