using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Api.Validators;
using CookApp.Application.Authentication;
using CookApp.Application.Authentication.DTOs;
using CookApp.Application.Interfaces.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CookApp.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(UserRegisterDTO userCredentials, [FromServices] IValidator<UserRegisterDTO> validator)
        {
            var validationResult = validator.Validate(userCredentials);
            if (!validationResult.IsValid)
            {
                foreach (var problem in validationResult.Errors)
                {
                    ModelState.AddModelError(nameof(problem.AttemptedValue), problem.ErrorMessage);
                }
                return ValidationProblem(ModelState);
            }

            await _userService.RegisterUser(userCredentials);

            return Ok();
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLoginDTO userCredentials, [FromServices] IValidator<UserLoginDTO> validator)
        {
            var validationResult = validator.Validate(userCredentials);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(nameof(error.AttemptedValue), error.ErrorMessage);
                }
                return ValidationProblem(ModelState);
            }

            string token = await _userService.LoginUser(userCredentials);

            return Ok(token);
        }
    }
}