using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductCatalog.Api.Controllers
{
    //we can think of creating a CustomBase Controller where we can injecct any common code we want to run for all requests like ExceptionFilter or AuthenticationFilter etc.
    [Route("user")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult FindUser()
        {
            return Ok(new UserResponseModel
            {
                Name = "John Smith",
                Token = "25a4f06f-8fd5-49b3-a711-c013c156f8c8"
            });
        }
    }

    public class UserResponseModel
    {
        [JsonProperty] public string Token { get; set; }

        [JsonProperty] public string Name { get; set; }
    }
}