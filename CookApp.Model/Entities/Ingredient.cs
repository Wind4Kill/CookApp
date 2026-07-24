using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.Entities
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        public string IngredientName { get; set; } = null!;

        public ICollection<Recipe> Recipes { get; set; } = null!;
    }
}