using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EBus.Publish.Controllers
{
    public class BaseController : ControllerBase
    {
        public readonly ILogger<BaseController> _logger;

        public readonly IServiceProvider _serviceProvider;

        public BaseController(ILogger<BaseController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

    }
}
