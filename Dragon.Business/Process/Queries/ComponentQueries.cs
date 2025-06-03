using Dragon.Data;
using Dragon.Model.Configs;
using Microsoft.EntityFrameworkCore;

namespace Dragon.Business.Process.Queries
{
    public static class ComponentQueries
    {
        public static readonly Func<DefaultContext, IEnumerable<ComponentStructure>> GetAllComponentStructure =
            EF.CompileQuery((DefaultContext defaultContext) =>
            defaultContext.ComponentStructure.AsNoTracking()
                .Include(d => d.Properties)
                    .ThenInclude(d => d.ComponentProperty));
    }
}
