using Dragon.API.Filters;
using Dragon.Business.Process.Configs;
using Dragon.Model;
using Dragon.Model.Configs;
using Microsoft.AspNetCore.Mvc;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Controllers.Configs
{
    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class NavigationController : BaseController
    {
        private readonly NavigationProcess process;
        public NavigationController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<Navigation>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] Navigation data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<Navigation>(pageData));
    }
}
