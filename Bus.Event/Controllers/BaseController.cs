using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EBus.Event.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IConfiguration Configuration;

        public BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

    }
}
