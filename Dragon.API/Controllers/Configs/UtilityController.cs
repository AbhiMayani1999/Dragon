using Dragon.API.Filters;
using Dragon.Business.Process.Configs;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using static Dragon.Provider.AccessProvider;
using static Dragon.Provider.ConnectionProvider;

namespace Dragon.API.Controllers.Configs
{
    [Route("api/[controller]")]
    public class UtilityController : BaseController
    {
        [HttpGet(nameof(MigrateData))] public async Task<IActionResult> MigrateData() => SendResponse(await UtilityProcess.MigrateAllDatabase());
        [HttpGet(nameof(BackupData))] public async Task<IActionResult> BackupData() => SendResponse(await UtilityProcess.BackupAllDatabase());
        [HttpGet(nameof(SeedData))] public async Task<IActionResult> SeedData() => SendResponse(await UtilityProcess.SeedAllDatabase());
        [HttpGet(nameof(GenerateAllPages))] public async Task<IActionResult> GenerateAllPages() => SendResponse(await UtilityProcess.GenerateAllPages());
        [HttpGet(nameof(MasterDomainConfig) + "/{data}")] public async Task<IActionResult> MasterDomainConfig(string data) => SendResponse(await UtilityProcess.MasterDomainConfig(data));

        [HttpPost(nameof(DecryptConnection))] public IActionResult DecryptConnection(string data) => SendResponse(new() { Status = (byte)StatusFlags.Success, Data = EncryptionProvider.Decrypt(data).FromJson<Connection>() });
        [HttpPost(nameof(EncryptConnection))] public IActionResult EncryptConnection(Connection data) => SendResponse(new() { Status = (byte)StatusFlags.Success, Data = EncryptionProvider.Encrypt(data.ToJson()) });

        [HttpGet(nameof(Export))]
        public async Task<IActionResult> Export()
        {
            try
            {
                GeneratorProcess process = new() { CurrentUser = new() { TenantCode = ConfigProvider.MasterTenantName } };
                string exportFolderName = $"{DateTime.Now:dd-MM-yyyy}";
                string downloadLocation = await process.Export(exportFolderName);
                string fileToGenerate = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, $"{exportFolderName}.zip");
                PathProvider.DeleteFile(fileToGenerate);
                ZipFile.CreateFromDirectory(downloadLocation, fileToGenerate);

                return new FileStreamResult(new FileStream(fileToGenerate, FileMode.Open, FileAccess.Read, FileShare.Read, 1024), FileProvider.ContentType(fileToGenerate));
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); return SendResponse(new ApiResponse { Status = (byte)StatusFlags.Failed, DetailedError = Convert.ToString(ex) }); }
        }
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class JobController : BaseController
    {
        private readonly JobProcess process;
        public JobController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet(nameof(Backup))] public async Task<IActionResult> Backup() => SendResponse(await process.Backup());
        [HttpGet(nameof(Migration))] public async Task<IActionResult> Migration() => SendResponse(await process.Migrate());
        [HttpGet(nameof(GeneratePages))] public async Task<IActionResult> GeneratePages() => SendResponse(await process.Pages());
    }
}