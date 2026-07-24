using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CookApp.Model.DTOs.RecipeDTOs
{
    [AutoMap(typeof(Recipe))]
    public class GetRecipeByIdDTO
    {
        public int RecipeId { get; set; }

        public string RecipeName { get; set; } = null!;

        public string[] Ingredients { get; set; } = null!;
    }
}