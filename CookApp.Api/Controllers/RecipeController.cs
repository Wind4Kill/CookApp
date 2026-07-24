using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model;
using CookApp.Model.DTOs;
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

        [HttpGet("{id:int}", Name ="GetRecipeById")]
        public async Task<ActionResult<GetRecipeByIdDTO>> GetRecipeById(int id)
        {
            GetRecipeByIdDTO requestedRecipe = await _recipeService.GetRecipeById(id);
            return Ok(requestedRecipe);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateRecipe(CreateRecipeDTO recipeDTO)
        {
            Recipe createdRecipe = await _recipeService.CreateRecipe(recipeDTO);
            return CreatedAtRoute("GetRecipeById", new { id = createdRecipe.RecipeId }, createdRecipe);
        }

    }
}