using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EBus.Event.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : BaseController
    {
        public ConsumerController(IConfiguration configuration) : base(configuration)
        {

        }

    }
}
