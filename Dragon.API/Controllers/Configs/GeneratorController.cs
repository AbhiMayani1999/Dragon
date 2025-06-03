using Dragon.API.Filters;
using Dragon.Business.Process.Configs;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Controllers.Configs
{
    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class GeneratorController : BaseController
    {
        private readonly GeneratorProcess process;
        public GeneratorController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpPost(nameof(Page))] public async Task<IActionResult> Page([FromBody] JObject data) => SendResponse(await process.Page(data), true);

        [HttpGet(nameof(Export))]
        public async Task<IActionResult> Export()
        {
            try
            {
                string exportFolderName = $"{DateTime.Now:dd-MM-yyyy}";
                string downloadLocation = await process.Export(exportFolderName);
                string fileToGenerate = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, $"{exportFolderName}.zip");
                PathProvider.DeleteFile(fileToGenerate); ZipFile.CreateFromDirectory(downloadLocation, fileToGenerate);
                return new FileStreamResult(new FileStream(fileToGenerate, FileMode.Open, FileAccess.Read, FileShare.Read, 1024), FileProvider.ContentType(fileToGenerate));
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); return SendResponse(new ApiResponse { Status = (byte)StatusFlags.Failed, DetailedError = Convert.ToString(ex) }); }
        }
    }
}
