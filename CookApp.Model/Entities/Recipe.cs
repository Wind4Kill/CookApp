using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.Entities;

namespace CookApp.Model
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        public string RecipeName { get; set; } = null!;

        public ICollection<Ingredient> Ingredients { get; set; } = null!;
    }
}