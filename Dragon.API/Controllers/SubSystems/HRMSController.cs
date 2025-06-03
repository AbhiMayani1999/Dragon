using Dragon.API.Filters;
using Dragon.Business.Process.SubSystems;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Model.SubSystems;
using Microsoft.AspNetCore.Mvc;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Controllers.SubSystems
{
    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsCompanyController : BaseController
    {
        private readonly HrmsCompanyProcess process;
        public HrmsCompanyController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsCompany>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsCompany data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsCompany>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsCompanyBankController : BaseController
    {
        private readonly HrmsCompanyBankProcess process;
        public HrmsCompanyBankController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsCompanyBank>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsCompanyBank data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsCompanyBank>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsCompanyDepartmentController : BaseController
    {
        private readonly HrmsCompanyDepartmentProcess process;
        public HrmsCompanyDepartmentController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsCompanyDepartment>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsCompanyDepartment data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsCompanyDepartment>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsEmployeeController : BaseController
    {
        private readonly HrmsEmployeeProcess process;
        public HrmsEmployeeController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsEmployee>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsEmployee data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsEmployee>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsEmployeeAppraisalController : BaseController
    {
        private readonly HrmsEmployeeAppraisalProcess process;
        public HrmsEmployeeAppraisalController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsEmployeeAppraisal>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsEmployeeAppraisal data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsEmployeeAppraisal>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsEmployeeSalaryController : BaseController
    {
        private readonly HrmsEmployeeSalaryProcess process;
        public HrmsEmployeeSalaryController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsEmployeeSalary>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsEmployeeSalary data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsEmployeeSalary>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsEmployeeDocumentController : BaseController
    {
        private readonly HrmsEmployeeDocumentProcess process;
        public HrmsEmployeeDocumentController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsEmployeeDocument>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsEmployeeDocument data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsEmployeeDocument>(pageData));
    }

    [AuthorizeRoles(SystemUserType.Master, SystemUserType.Admin), Route("api/[controller]")]
    public class HrmsEmployeeBankController : BaseController
    {
        private readonly HrmsEmployeeBankProcess process;
        public HrmsEmployeeBankController([FromServices] User user) { process = new() { CurrentUser = user }; }
        [HttpGet("{id?}")] public async Task<IActionResult> Get(int? id = null) => SendResponse(await process.Get<HrmsEmployeeBank>(id ?? 0));
        [HttpPost] public async Task<IActionResult> Post([FromBody] HrmsEmployeeBank data) => SendResponse(await process.Save(data), true);
        [HttpDelete("{id}")] public async Task<IActionResult> Delete([FromRoute] int id) => SendResponse(await process.Delete(id), true);
        [HttpPost(nameof(GetPage))] public async Task<IActionResult> GetPage([FromBody] PageData pageData) => SendResponse(await process.GetPage<HrmsEmployeeBank>(pageData));
    }
}
