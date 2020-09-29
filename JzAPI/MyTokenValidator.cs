using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JzAPI
{
    /// <summary>
    /// token验证类
    /// </summary>
    [EnableCors("CorsPolicy")]
    public class MyTokenValidator : ISecurityTokenValidator
    {
        bool ISecurityTokenValidator.CanValidateToken => true;

        int ISecurityTokenValidator.MaximumTokenSizeInBytes { get; set; }

        bool ISecurityTokenValidator.CanReadToken(string securityToken)
        {
            return true;
        }

        //验证token
        ClaimsPrincipal ISecurityTokenValidator.ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ClaimsPrincipal principal;
            try
            {
                validatedToken = null;
                
                var token = new JwtSecurityToken(securityToken);
                //获取到Token的一切信息
                var payload = token.Payload;
                var role = (from t in payload where t.Key == ClaimTypes.Role select t.Value).FirstOrDefault();
                var name = (from t in payload where t.Key == ClaimTypes.Name select t.Value).FirstOrDefault();
                var id  = (from t in payload where t.Key == ClaimTypes.Sid select t.Value).FirstOrDefault();
                var exp = (from t in payload where t.Key == "exp" select t.Value).FirstOrDefault();
                if (IntToDateTime(int.Parse(exp.ToString())) < DateTime.Now)
                {
                    throw new Exception();
                }
                var issuer = token.Issuer;
                var key = token.SecurityKey;
                var audience = token.Audiences;
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, name.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType,role.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Sid, id.ToString()));
                principal = new ClaimsPrincipal(identity);
            }
            catch
            {
                validatedToken = null;
                principal = null;
            }
            return principal;
        }
        public DateTime IntToDateTime(int timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(timestamp);
        }
    }

}
