using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Application.FiltrationClasses;
using CookApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace CookApp.Api.Controllers
{
    [ApiController]
    [Route("api/Recipes")]
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
        [ProducesResponseType<List<GetRecipeDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [OutputCache(Duration =120, Tags = new[] { "all-recipes" })]
        public async Task<ActionResult> GetRecipies([FromQuery]FiltrationDTO filterOptions, CancellationToken token)
        {
            Filter filter = new Filter(filterOptions.FiltrationOrder!, filterOptions.FiltrationType!, filterOptions.FiltrationData, filterOptions.Page);
            List<GetRecipeDTO> result = await _recipeService.GetRecipes(filter, token);

            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetRecipeById")]
        [Produces("application/json")]
        [ProducesResponseType<GetRecipeByIdDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> GetRecipeById(int id, CancellationToken token)
        {
            GetRecipeByIdDTO requestedRecipe = await _recipeService.GetRecipeById(id, token);

            return Ok(requestedRecipe);
        }

        [HttpPost("")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType<GetRecipeByIdDTO>(StatusCodes.Status201Created)]

        public async Task<ActionResult> CreateRecipe(CreateRecipeDTO recipeDTO, CancellationToken token)
        {
            GetRecipeByIdDTO createdRecipe = await _recipeService.CreateRecipe(recipeDTO, token);
            await _store.EvictByTagAsync("all-recipes", token);

            return CreatedAtRoute("GetRecipeById", new { id = createdRecipe.RecipeId }, createdRecipe);
        }

        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteRecipe(int id, CancellationToken token)
        {
            await _recipeService.DeleteRecipe(id, token);
            await _store.EvictByTagAsync("all-recipes", token);

            return NoContent();
        }

    }
}