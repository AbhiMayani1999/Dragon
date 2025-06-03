using Dragon.Business.Process;
using Dragon.Model;
using Microsoft.AspNetCore.Mvc;

namespace Dragon.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [HttpPost] public async Task<IActionResult> Post([FromBody] AuthModel data) => SendResponse(await LoginProcess.AuthMech(Request.Headers.FirstOrDefault(d => d.Key == "Origin").Value.FirstOrDefault(), data));
    }
}
