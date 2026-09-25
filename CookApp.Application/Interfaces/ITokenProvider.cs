using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookApp.Application.Interfaces
{
    public interface ITokenProvider
    {
        string GenerateAccessToken(List<Claim> claims);

        string GenerateRefreshToken();
    }
}