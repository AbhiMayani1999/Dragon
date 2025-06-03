using Dragon.Data;
using Dragon.Model.SubSystems;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.Queries
{
    public static class HrmsQueries
    {
        public static readonly Func<HrmsContext, IEnumerable<HrmsEmployee>> GetAllEmployees =
            EF.CompileQuery((HrmsContext hrmsContext) =>
            hrmsContext.HrmsEmployee.AsNoTracking()
                .Include(d => d.Appraisals)
                .Include(d => d.Documents)
                .Include(d => d.Salaries)
                .Include(d => d.Banks)
                .Include(d => d.Department)
                    .ThenInclude(d => d.Company));
    }
}
