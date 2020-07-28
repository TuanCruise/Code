using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EBus.Publish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseController : BaseController
    {
        public WareHouseController(ILogger<WareHouseController> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {

        }
    }
}
