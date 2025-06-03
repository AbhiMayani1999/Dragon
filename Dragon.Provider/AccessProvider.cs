using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dragon.Provider
{
    public static class AccessProvider
    {
        public enum SystemUserType { Master, Admin, User, Subscriber, General }
        public static bool RetrieveUserFromAccessToken(ref string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                byte[] AuthKey = Encoding.ASCII.GetBytes(ConfigProvider.EncryptionKey);
                JwtSecurityTokenHandler tokenHandler = new();
                TokenValidationParameters tokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(AuthKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(data, tokenValidationParameters, out SecurityToken tokenSecure);
                Claim claim = claimsPrincipal.Claims.Where(d => d.Type == ClaimTypes.UserData).FirstOrDefault();
                if (claim != null && !string.IsNullOrWhiteSpace(claim.Value)) { data = claim.Value; return true; } else { data = string.Empty; }
            }
            return false;
        }
        public static string GetUserAccessToken(object users, DateTime validityTill, Claim additionalClaim = null)
        {
            byte[] AuthKey = Encoding.ASCII.GetBytes(ConfigProvider.EncryptionKey);
            JwtSecurityTokenHandler tokenHandler = new();
            List<Claim> claims = [new Claim(ClaimTypes.UserData, users.ToJson())];
            if (additionalClaim != null) { claims.Add(additionalClaim); }
            return tokenHandler.WriteToken(tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = validityTill,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(AuthKey),
                    SecurityAlgorithms.HmacSha256Signature)
            }));
        }
    }
}