using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CookApp.Api.Controllers
{
    [ApiController]
    [Route("api/Recipies")]
    public class RecipeController : ControllerBase
    {
        readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<GetRecipeDTO>>> GetRecipies()
        {
            List<GetRecipeDTO> result = await _recipeService.GetRecipes();
            return Ok(result);
        }
    }
}