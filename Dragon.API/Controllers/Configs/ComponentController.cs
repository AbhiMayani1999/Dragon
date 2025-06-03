using Dragon.API.Filters;
using Dragon.Business.Process.Configs;
using Dragon.Model;
using Dragon.Model.Configs;
using Microsoft.AspNetCore.Mvc;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Controllers.Configs
{
    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class ComponentStructureController : BaseController
    {
        private readonly ComponentStructureProcess process;
        public ComponentStructureController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] ComponentStructure data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class ComponentPropertyController : BaseController
    {
        private readonly ComponentPropertyProcess process;
        public ComponentPropertyController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<ComponentProperty>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] ComponentProperty data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class StructureController : BaseController
    {
        private readonly StructureProcess process;
        public StructureController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] Structure data) => SendResponse(await process.Save(data), true);
        [HttpPost(nameof(ReIndex))] public async Task<IActionResult> ReIndex([FromBody] List<ReindexStruct> data) => SendResponse(await process.ReIndex(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class StructurePropertyController : BaseController
    {
        private readonly StructurePropertyProcess process;
        public StructurePropertyController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] StructureProperty data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
    }
}
