using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CookApp.Application.Authentication;
using CookApp.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CookApp.Data.Authentication
{
    public class JwtTokenProvider(IOptions<JwtTokenSettings> settings) : ITokenProvider
    {
        public string GenerateAccessToken(List<Claim> claims)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Audience = settings.Value.Issued,
                Issuer = settings.Value.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(settings.Value.Expiration),
                SigningCredentials = signingCredentials

            };
            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return token;
        }

        public string GenerateRefreshToken()
        {
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            return refreshToken;
        }
    }
}