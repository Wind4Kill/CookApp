using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.Exceptions
{
    public class UserWrongDataException : Exception
    {
        public UserWrongDataException(string message) : base(message) { }
    }
}