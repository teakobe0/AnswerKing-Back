using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace JzAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Base")]
    public class BaseController : Controller
    {
        public string Email
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.Name);
            }
        }

        public int ID
        {
            get
            {
                return int.Parse(User.FindFirstValue(ClaimTypes.Sid));
            }
        }

        public string Role
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.Role);
            }
        }


        protected object getToken(TokenParam param)
        {
            var claims = new[]
               {
                   new Claim(ClaimTypes.Sid,param.Id.ToString()),
                   new Claim(ClaimTypes.Name, param.Username),
                   new Claim(ClaimTypes.Role,param.Role)

               };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(param.configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(1440),
                signingCredentials: creds);

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public class TokenRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            
        }

        public class TokenParam
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public IConfiguration configuration { get; set; }
        }

    }
}