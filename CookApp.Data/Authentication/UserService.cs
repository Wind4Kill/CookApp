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
using CookApp.Model.Entities.UserClasses;
using CookApp.Model.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CookApp.Data.Authentication
{
    public class UserService(ILogger<UserService> logger, IHttpContextAccessor httpContext, ApplicationContext dbContext, UserManager<User> userManager, ITokenProvider tokenProvider) : IUserService
    {
        public async Task RegisterUser(UserRegisterDTO userCredentials)
        {

            logger.LogInformation("Registration of {Email} started.", userCredentials.Email);

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
                    logger.LogError("Registration of {Email} failed.", userCredentials.Email);
                    throw new ValidationException($"Failed user registration: {errors}");
                }
                await userManager.AddClaimsAsync(user, claims);

                await transaction.CommitAsync();
            });

            logger.LogInformation("User {Email} registered", userCredentials.Email);
        }

        public async Task<TokensResponseDTO> LoginUser(UserLoginDTO userCredentials)
        {
            logger.LogInformation("User {Email} loging started.", userCredentials.Email);
            
            var requestedUser = await userManager.FindByEmailAsync(userCredentials.Email);
            if (requestedUser is null)
            {
                logger.LogError("User {Email} wasn't found.", userCredentials.Email);
                throw new UserNotFound("Such user wasn't found.");
            }
            var validationResult = await userManager.CheckPasswordAsync(requestedUser, userCredentials.Password);
            if (!validationResult)
            {
                logger.LogError("User {Email} credentials {@userCredentials} validation failed.", userCredentials.Email, userCredentials);
                throw new UserWrongDataException("Provided user credentials are not valid.");
            }

            await dbContext.Entry(requestedUser).Collection(u => u.RefreshTokens).LoadAsync();

            if (requestedUser.RefreshTokens is not null)
            {
                dbContext.RefreshTokens.RemoveRange(requestedUser.RefreshTokens);
            }

            List<Claim> userClaims = (await userManager.GetClaimsAsync(requestedUser)).ToList();

            userClaims.AddRange(new Claim("Id", requestedUser.Id), new Claim("Email", requestedUser.Email!), new Claim("UserName", requestedUser.UserName!));

            string accessToken = tokenProvider.GenerateAccessToken(userClaims);
            string refreshToken = tokenProvider.GenerateRefreshToken();

            RefreshToken refreshTokenEntity = new RefreshToken()
            {
                RefreshTokenId = Guid.NewGuid().ToString(),
                Token = refreshToken,
                User = requestedUser,
                Expiration = DateTime.UtcNow.AddDays(3)
            };

            dbContext.RefreshTokens.Add(refreshTokenEntity);

            TokensResponseDTO tokens = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            await dbContext.SaveChangesAsync();

            return tokens;
        }

        public async Task<TokensResponseDTO> RefreshTokens(string refreshToken)
        {
            RefreshToken? requestedToken = await dbContext.RefreshTokens.Include(rt => rt.User).SingleOrDefaultAsync(rt => rt.Token == refreshToken);

            if (requestedToken is null || requestedToken.Expiration < DateTime.UtcNow)
            {
                logger.LogError("Refresh token for user {@User} expired.", requestedToken?.User);
                throw new InvalidOperationException("Token expired.");
            }

            if (!ValidateUserId(requestedToken.UserId))
            {
                logger.LogError("User id {ActualId} access denied for refreshing tokens of user {RequestedId}",
                httpContext.HttpContext.User.FindFirstValue("Id"), requestedToken.UserId);

                throw new ValidationException("You are not permitted for this action.");
            }

            var claims = (await userManager.GetClaimsAsync(requestedToken.User)).ToList();

            string accessToken = tokenProvider.GenerateAccessToken(claims);
            requestedToken.Token = tokenProvider.GenerateRefreshToken();
            requestedToken.Expiration = DateTime.UtcNow.AddDays(3);

            await dbContext.SaveChangesAsync();

            TokensResponseDTO tokens = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return tokens;
        }

        private bool ValidateUserId(string userId)
        {
            bool isMatch = httpContext.HttpContext.User.FindFirstValue("Id") == userId;
            return isMatch;
        }
    }
}