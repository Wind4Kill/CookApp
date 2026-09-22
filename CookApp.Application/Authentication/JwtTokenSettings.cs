using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Application.Authentication
{
    public class JwtTokenSettings
    {
        public string Issuer { get; set; } = null!;
        public string Issued { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public int Expiration { get; set; }
    }
}