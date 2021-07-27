using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspSample.App.Main.Controllers
{
    [ApiController]
    [Route("api/home")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private ILogger<HomeController> Logger { get; }

        public HomeController(ILogger<HomeController> logger)
        {
            Logger = logger;
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
