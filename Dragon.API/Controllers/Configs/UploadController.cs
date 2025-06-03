using Dragon.Business;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace Dragon.API.Controllers.Configs
{
    [Authorize, Route("file/[controller]"), DisableRequestSizeLimit]
    public class TempController : BaseController
    {
        [HttpGet("/{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            try { return File(await System.IO.File.ReadAllBytesAsync(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, name)), FileProvider.ContentType(name)); }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); return SendResponse(new ApiResponse { Status = (byte)StatusFlags.Failed, DetailedError = Convert.ToString(ex) }); }
        }

        [HttpPost, Consumes("multipart/form-data")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try { apiResponse.Data = await FileProvider.ReadFileToPath(files.FirstOrDefault(), Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName)); }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return SendResponse(apiResponse);
        }

        [HttpDelete("{name}")]
        public IActionResult Delete([FromRoute] string name)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try { PathProvider.DeleteFile(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, name)); }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return SendResponse(apiResponse);
        }

        [HttpPost(nameof(Download) + "/{name}")]
        public IActionResult Download([FromRoute] string name)
        {
            try { return new FileStreamResult(new FileStream(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, name), FileMode.Open, FileAccess.Read, FileShare.Read, 1024), FileProvider.ContentType(name)); }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); return SendResponse(new ApiResponse { Status = (byte)StatusFlags.Failed, DetailedError = Convert.ToString(ex) }); }
        }
    }

    [Authorize, Route("file/[controller]")]
    public class TenantController : BaseController
    {
        private readonly GlobalVariables process;
        public TenantController([FromServices] User user = null) { process = new() { CurrentUser = user }; }

        [HttpGet("/{filetype}/{name}")]
        public async Task<IActionResult> Get([FromRoute] string filetype, [FromRoute] string name)
        {
            try
            {
                string folderLocation = string.Empty;
                if (FunctionProvider.IsEqualString(filetype, "image")) { folderLocation = ConfigProvider.Settings.ImageFolderName; }
                return File(await System.IO.File.ReadAllBytesAsync(Path.Combine(ConfigProvider.Provider.BaseDirectory, process.CurrentUser.TenantCode, folderLocation, name)), FileProvider.ContentType(name));
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); return SendResponse(new ApiResponse { Status = (byte)StatusFlags.Failed, DetailedError = Convert.ToString(ex) }); }
        }

        [HttpPost(nameof(Download) + "/{name?}")]
        public IActionResult Download([FromRoute] string name, [FromRoute] string[] path = null)
        {
            try
            {
                string downloadPath = process.GetFolderLocation(name);
                if (!string.IsNullOrWhiteSpace(name)) { downloadPath = Path.Combine(downloadPath, name); }
                else
                {
                    string fileToGenerate = Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, $"{path.Last()}.zip");
                    PathProvider.DeleteFile(fileToGenerate);
                    ZipFile.CreateFromDirectory(downloadPath, fileToGenerate);
                    downloadPath = fileToGenerate;
                }
                return new FileStreamResult(new FileStream(downloadPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024), FileProvider.ContentType(downloadPath));
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); return SendResponse(new ApiResponse { Status = (byte)StatusFlags.Failed, DetailedError = Convert.ToString(ex) }); }
        }

        [HttpDelete("{name}")]
        public IActionResult Delete([FromRoute] string name)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Success };
            try { PathProvider.DeleteFile(Path.Combine(ConfigProvider.Provider.BaseDirectory, ConfigProvider.Settings.TempFolderName, name)); }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), null); apiResponse.Status = (byte)StatusFlags.Failed; apiResponse.DetailedError = Convert.ToString(ex); }
            return SendResponse(apiResponse);
        }
    }
}
