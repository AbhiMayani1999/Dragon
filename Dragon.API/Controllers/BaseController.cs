using Dragon.Enm;
using Dragon.Model;
using Dragon.Provider;
using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
namespace Dragon.API.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public ActionResult SendResponse(ApiResponse apiResponse, bool showMessage = false)
        {
            if (showMessage) { apiResponse.Message ??= Convert.ToString(Enum.Parse<StatusFlags>(Convert.ToString(apiResponse.Status))).AddSpaceBeforeCapital(); }
            return apiResponse.Status == (byte)StatusFlags.Failed ? BadRequest(apiResponse) : Ok(apiResponse);
        }
    }
}