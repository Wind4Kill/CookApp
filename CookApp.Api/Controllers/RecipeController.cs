using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model;
using CookApp.Model.DTOs;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.FiltrationClasses;
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
        [Produces("application/json")]
        public async Task<ActionResult<List<GetRecipeDTO>>> GetRecipies([FromQuery]FiltrationDTO filterOptions)
        {
            Filter filter = new Filter(filterOptions.FiltrationOrder!, filterOptions.FiltrationType!, filterOptions.FiltrationData, filterOptions.Page);
            List<GetRecipeDTO> result = await _recipeService.GetRecipes(filter);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetRecipeById")]
        public async Task<ActionResult<GetRecipeByIdDTO>> GetRecipeById(int id)
        {
            GetRecipeByIdDTO requestedRecipe = await _recipeService.GetRecipeById(id);
            return Ok(requestedRecipe);
        }

        [HttpPost("")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> CreateRecipe(CreateRecipeDTO recipeDTO)
        {
            Recipe createdRecipe = await _recipeService.CreateRecipe(recipeDTO);
            return CreatedAtRoute("GetRecipeById", new { id = createdRecipe.RecipeId }, createdRecipe);
        }

        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            int result = await _recipeService.DeleteRecipe(id);

            return NoContent();
        }

    }
}