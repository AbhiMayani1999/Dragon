using Dragon.API.Filters;
using Dragon.Business.Process.Configs;
using Dragon.Model;
using Dragon.Model.Configs;
using Microsoft.AspNetCore.Mvc;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Controllers.Configs
{
    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly UserProcess process;
        public UserController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<User>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] User data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<User>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class UserTypeController : BaseController
    {
        private readonly UserTypeProcess process;
        public UserTypeController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<UserType>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] UserType data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<UserType>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class UserNavigationController : BaseController
    {
        private readonly UserNavigationProcess process;
        public UserNavigationController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] List<UserNavigation> data) => SendResponse(await process.Save(data), true);
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class UserTypeNavigationController : BaseController
    {
        private readonly UserTypeNavigationProcess process;
        public UserTypeNavigationController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] List<UserTypeNavigation> data) => SendResponse(await process.Save(data), true);
    }
}
