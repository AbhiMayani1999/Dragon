using Dragon.Data;
using Dragon.Enm;
using Dragon.Model;
using Dragon.Model.Configs;
using Dragon.Provider;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Claims;
using static Dragon.Provider.AccessProvider;
using static Dragon.Provider.ConnectionProvider;

namespace Dragon.Business.Process
{
    public class LoginProcess
    {
        public static async Task<ApiResponse> AuthMech(string origin, AuthModel data)
        {
            ApiResponse apiResponse = new() { Status = (byte)StatusFlags.Failed };
            try
            {
                if (string.IsNullOrWhiteSpace(data.TenantCode) && FunctionProvider.IsEmail(data.Username))
                {
                    MailAddress email = new(data.Username);
                    data.TenantCode = await GetTenantCode(email.Host);
                    if (string.IsNullOrWhiteSpace(data.TenantCode)) { apiResponse.Message = "Email not linked to any Tenant"; }
                }

                if (string.IsNullOrWhiteSpace(data.TenantCode) && !string.IsNullOrWhiteSpace(origin))
                {
                    data.TenantCode = await GetTenantCode(origin);
                    if (string.IsNullOrWhiteSpace(data.TenantCode)) { apiResponse.Message += "Origin not linked to any Tenant"; }
                }

                if (!string.IsNullOrWhiteSpace(data.TenantCode))
                {
                    Connection connection = ConnectionProvider.Provider.GetConnection(data.TenantCode);
                    if (connection != null && connection.IsActive && !connection.IsDeleted)
                    {
                        DefaultContext defaultContext = new(connection);
                        User user = await defaultContext.User.AsNoTracking().Include(d => d.UserType)
                                .FirstOrDefaultAsync(u => u.Username == data.Username && u.Password == EncryptionProvider.Encrypt(data.Password) && u.IsDeleted == false);

                        if (user == null) { apiResponse.Message = "Enter valid credentials"; }
                        else if (!user.IsActive) { apiResponse.Message = "User inactivated"; }
                        else
                        {
                            user.Password = ""; user.TenantCode = data.TenantCode; DateTime expiry = DateTime.Now.Add(TimeSpan.FromHours(24) - DateTime.Now.TimeOfDay); user.UserTypeName = user.UserType.Name;
                            Claim additionalClaim = new(ClaimTypes.Role, user.UserType.IsAdmin ? Convert.ToString(SystemUserType.Admin) : Convert.ToString(SystemUserType.User)); user.UserType = null;
                            apiResponse = new ApiResponse { Data = GetUserAccessToken(user, expiry, additionalClaim), Status = (byte)StatusFlags.Success };
                        }
                    }
                    else { apiResponse.Message = "Tenant not found"; }
                }
            }
            catch (Exception ex) { LogsProvider.WriteErrorLog(Convert.ToString(ex), data); apiResponse.DetailedError = Convert.ToString(ex); }
            return apiResponse;
        }
        public static async Task<string> GetTenantCode(string origin)
        {
            string TenantCode = string.Empty;
            origin = FunctionProvider.GetOrigin(origin);
            DefaultContext defaultContext = new(ConnectionProvider.Provider.GetConnection(ConfigProvider.MasterTenantName));
            DomainConnect connectionDomain = await defaultContext.DomainConnect.AsNoTracking().FirstOrDefaultAsync(d => d.Name.ToLower() == origin.ToLower());

            if (connectionDomain != null) { TenantCode = !string.IsNullOrWhiteSpace(connectionDomain.TenantCode) ? connectionDomain.TenantCode : string.Empty; }
            else { defaultContext.DomainConnect.Add(new DomainConnect { Name = origin.ToLower() }); await defaultContext.SaveChangesAsync(); }
            return TenantCode;
        }
    }
}
