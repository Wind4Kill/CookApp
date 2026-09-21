using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.Authentication.DTOs;
using CookApp.Application.Interfaces.Authentication;
using CookApp.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace CookApp.Data.Authentication
{
    public class UserService(UserManager<User> userManager) : IUserService
    {

        public async Task RegisterUser(UserRegisterDTO userCredentials)
        {
            User user = new User(userName: userCredentials.Login)
            {
                Email = userCredentials.Email
            };
            await userManager.CreateAsync(user, userCredentials.Password);
        }
    }
}