using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspSample.App.Main.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("hello")]
        [HttpPost]
        public HomeHelloRes Hello([FromBody] HomeHelloReq json)
        {
            return new HomeHelloRes
            (
                Hello: $"Hello, {json.Name}!"
            );
        }
    }

    public record HomeHelloReq
    (
        string Name
    );

    public record HomeHelloRes
    (
        string Hello
    );
}
