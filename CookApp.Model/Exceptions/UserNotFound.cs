using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.Exceptions
{
    public class UserNotFound : Exception
    {
        public UserNotFound(string message) : base(message) {}
    }
}