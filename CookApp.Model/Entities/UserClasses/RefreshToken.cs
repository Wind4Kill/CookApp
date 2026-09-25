using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.Entities.UserClasses
{
    public class RefreshToken
    {
        public string RefreshTokenId { get; set; } = null!;

        public string Token { get; set; } = null!;
        
        public DateTime Expiration { get; set; }

        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}