
namespace CookApp.Model.Entities;
public class Recipe
{
    public DateOnly CreatedAt { get; private set; }

    public int RecipeId { get; set; }

    public string RecipeName { get; set; } = null!;

    public List<Ingredient> Ingredients { get; set; } = null!;

    public bool IsDeleted { get; set; }

}