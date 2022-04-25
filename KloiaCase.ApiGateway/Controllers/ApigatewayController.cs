using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KloiaCase.ApiGateway.Controllers
{
    [ApiController]
    [Route("apigateway")]
    public class ApigatewayController : ControllerBase
    {
        private readonly ILogger<ApigatewayController> _logger;

        public ApigatewayController(ILogger<ApigatewayController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("apigateway")]
        public bool Get()
        {
            return true;
        }
    }
}
