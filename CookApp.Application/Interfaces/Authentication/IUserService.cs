using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.Authentication;
using CookApp.Application.Authentication.DTOs;

namespace CookApp.Application.Interfaces.Authentication
{
    public interface IUserService
    {
        Task RegisterUser(UserRegisterDTO userCredentials);
        Task<TokensResponseDTO> LoginUser(UserLoginDTO userCredentials);
        Task<TokensResponseDTO> RefreshTokens(string refreshToken);
    }
}