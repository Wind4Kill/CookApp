using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CookApp.Model.Entities.UserClasses
{
    public class User:IdentityUser
    {
        public User(string userName) : base(userName) { }
        public User(){}
        public ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}