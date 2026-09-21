using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Api.Validators;
using CookApp.Application.Authentication.DTOs;
using CookApp.Application.Interfaces.Authentication;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CookApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/Register")]
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

        // [HttpPost("/Login")]
        // public async Task<IActionResult> Login()
        // {

        // }
    }
}