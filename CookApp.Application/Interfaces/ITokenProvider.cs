using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookApp.Application.Interfaces
{
    public interface ITokenProvider
    {
        string GenerateToken(Dictionary<string, string> claims);
    }
}