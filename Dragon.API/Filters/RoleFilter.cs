using Microsoft.AspNetCore.Authorization;
using System.Data;
using static Dragon.Provider.AccessProvider;

namespace Dragon.API.Filters
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params SystemUserType[] allowedRoles)
        {
            IEnumerable<string> allowedRolesAsStrings = allowedRoles.Select(a => Enum.GetName(typeof(SystemUserType), a));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
