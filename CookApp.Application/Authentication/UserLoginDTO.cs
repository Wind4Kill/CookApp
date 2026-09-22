using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Application.Authentication
{
    public class UserLoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}