using Dragon.API.Filters;
using Microsoft.AspNetCore.Mvc;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Controllers.Configs
{
    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class TransferController : BaseController
    {
        //private readonly ConnectionProcess process;
        //public TransferController([FromServices] User user) { process = new() { CurrentUser = user }; }
        //[HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<Connection>(id ?? 0));
        //[HttpPost] public async Task<IActionResult> Post([FromBody] Connection data) => SendResponse(await process.Save(data), true);
        //[HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        //[HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<Connection>(pageData));
    }
}
