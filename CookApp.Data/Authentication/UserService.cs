using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookApp.Application.Authentication;
using CookApp.Application.Authentication.DTOs;
using CookApp.Application.Interfaces;
using CookApp.Application.Interfaces.Authentication;
using CookApp.Model.Entities;
using CookApp.Model.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CookApp.Data.Authentication
{
    public class UserService(ApplicationContext dbContext, UserManager<User> userManager, ITokenProvider tokenProvider) : IUserService
    {
        public async Task RegisterUser(UserRegisterDTO userCredentials)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                User user = new User(userName: userCredentials.Login)
                {
                    Email = userCredentials.Email
                };
                List<Claim> claims = new()
                {
                    new Claim("Role", "User")
                };
                var createdResult = await userManager.CreateAsync(user, userCredentials.Password);
                if (!createdResult.Succeeded)
                {
                    string errors = string.Join(", ", createdResult.Errors);
                    throw new ValidationException($"Failed user registration: {errors}");
                }
                await userManager.AddClaimsAsync(user, claims);

                await transaction.CommitAsync();
            });
        }

        public async Task<string> LoginUser(UserLoginDTO userCredentials)
        {
            var requestedUser = await userManager.FindByEmailAsync(userCredentials.Email);
            if (requestedUser is null)
            {
                throw new UserNotFound("Such user wasn't found.");
            }
            var validationResult = await userManager.CheckPasswordAsync(requestedUser, userCredentials.Password);
            if (!validationResult)
            {
                throw new UserWrongDataException("Provided user credentials are not valid.");
            }

            var userClaims = await userManager.GetClaimsAsync(requestedUser);

            Dictionary<string, string> claimsDictionary = new()
            {
                {"Id", requestedUser.Id },
                {"Email", requestedUser.Email! },
                {"UserName", requestedUser.UserName! }
            };

            foreach(var claim in userClaims)
            {
                claimsDictionary.Add(claim.Type, claim.Value);
            }

            string token = tokenProvider.GenerateToken(claimsDictionary);
            return token;
        }
    }
}