using Dragon.API.Filters;
using Dragon.Business.Process.Configs;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Dragon.Provider.AccessProvider;
using static Dragon.Provider.ConnectionProvider;

namespace Dragon.API.Controllers.Configs
{
    [Authorize, Route("api/[controller]")]
    public class ConfigController : BaseController
    {
        private readonly NavigationProcess navigationProcess;
        public ConfigController([FromServices] User user) { navigationProcess = new() { CurrentUser = user }; }
        [HttpGet(nameof(MyNavigations))] public async Task<IActionResult> MyNavigations() => SendResponse(await navigationProcess.GetUserPermittedNavigation());
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class ConnectionController : BaseController
    {
        private readonly ConnectionProcess process;
        public ConnectionController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<Connection>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] Connection data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<Connection>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class KeyGroupController : BaseController
    {
        private readonly KeyGroupProcess process;
        public KeyGroupController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<KeyGroup>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] KeyGroup data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<KeyGroup>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class KeyStoreController : BaseController
    {
        private readonly KeyStoreProcess process;
        public KeyStoreController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<KeyStore>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] KeyStore data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<KeyStore>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class DomainConnectController : BaseController
    {
        private readonly DomainConnectProcess process;
        public DomainConnectController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<DomainConnect>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] DomainConnect data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<DomainConnect>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class DomainSettingController : BaseController
    {
        private readonly DomainSettingProcess process;
        public DomainSettingController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<DomainSetting>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] DomainSetting data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<DomainSetting>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class EmailConfigController : BaseController
    {
        private readonly EmailConfigProcess process;
        public EmailConfigController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<EmailConfig>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] EmailConfig data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<EmailConfig>(pageData));
    }
}
