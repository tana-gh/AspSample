using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspSample.App.Main.Controllers
{
    [ApiController]
    [Route("api/key")]
    public class KeyController : ControllerBase
    {
        private ILogger<HomeController> Logger { get; }
        private IConfiguration Configuration { get; }

        public KeyController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            Logger = logger;
            Configuration = configuration;
        }

        [Route("firebase")]
        [HttpGet]
        public KeyFirebaseRes Firebase()
        {
            return JsonConvert.DeserializeObject<KeyFirebaseRes>(System.IO.File.ReadAllText(Configuration["FirebaseKeyPath"]));
        }
    }

    public record KeyFirebaseRes
    (
        string ApiKey,
        string AuthDomain,
        string ProjectId,
        string StorageBucket,
        string MessagingSenderId,
        string AppId,
        string MeasurementId
    );
}
