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
using Microsoft.AspNetCore.OutputCaching;

namespace CookApp.Api.Controllers
{
    [ApiController]
    [Route("api/Recipies")]
    public class RecipeController : ControllerBase
    {
        readonly IRecipeService _recipeService;
        readonly IOutputCacheStore _store;
        public RecipeController(IRecipeService recipeService, IOutputCacheStore store)
        {
            _recipeService = recipeService;
            _store = store;
        }

        [HttpGet("")]
        [Produces("application/json")]
        [OutputCache(Duration =120, Tags = new[] { "all-books" })]
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
            await _store.EvictByTagAsync("all-books", default);

            return CreatedAtRoute("GetRecipeById", new { id = createdRecipe.RecipeId }, createdRecipe);
        }

        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            int result = await _recipeService.DeleteRecipe(id);
            await _store.EvictByTagAsync("all-books", default);

            return NoContent();
        }

    }
}