using Dragon.Business.Process.Configs;
using Dragon.Model;
using Dragon.Provider;
using Microsoft.AspNetCore.Mvc;

namespace Dragon.API.Controllers.Configs
{
    [Route("api/[controller]")]
    public class OptionController : BaseController
    {
        private readonly OptionProcess process = new() { CurrentUser = new() { TenantCode = ConfigProvider.MasterTenantName } };
        [HttpPost] public IActionResult Post([FromBody] List<OptionsTransfer> data) => SendResponse(process.FillMultipleOptions(data));
    }
}