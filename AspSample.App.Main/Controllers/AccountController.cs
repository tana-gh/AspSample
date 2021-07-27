using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspSample.App.Main.Controllers
{
    [ApiController]
    [Route("api/account")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private ILogger<AccountController> Logger { get; }

        public AccountController(ILogger<AccountController> logger)
        {
            Logger = logger;
        }

        [Route("login")]
        [HttpGet]
        public AccountRes Login()
        {
            return new AccountRes
            (
                Result: "Success"
            );
        }
    }

    public record AccountRes
    (
        string Result
    );
}
