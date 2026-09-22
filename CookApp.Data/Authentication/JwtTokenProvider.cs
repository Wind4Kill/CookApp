using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CookApp.Application.Authentication;
using CookApp.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CookApp.Data.Authentication
{
    public class JwtTokenProvider(IOptions<JwtTokenSettings> settings) : ITokenProvider
    {
        public string GenerateToken(Dictionary<string, string> claims)
        {
            Dictionary<string, object> userClaims = new()
            {
                {JwtRegisteredClaimNames.Sub, claims["Id"]},
                {JwtRegisteredClaimNames.Email, claims["Email"] },
                {JwtRegisteredClaimNames.Name, claims["UserName"] },
                {"role", claims["Role"]}
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor()
            {
                Claims = userClaims,
                Audience = settings.Value.Issued,
                Issuer = settings.Value.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(settings.Value.Expiration),
                SigningCredentials = signingCredentials

            };
            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return token;
        }
    }
}