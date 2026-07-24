using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Model.DTOs.RecipeDTOs;
using CookApp.Model.Entities;

namespace CookApp.Model.MapProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {

            CreateMap<Ingredient, string>().ConvertUsing(ingr => ingr.IngredientName);

            CreateMap<Recipe, GetRecipeDTO>().
            ForMember(
            dest => dest.Ingredients,
            opt => opt.MapFrom(src => src.Ingredients.Select(i => i.IngredientName
            )));
        }
    }
}