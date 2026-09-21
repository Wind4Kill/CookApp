using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Application.DTOs.RecipeDTOs;
using CookApp.Model.Entities;

namespace CookApp.Application.MapProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {

            CreateMap<Ingredient, string>().ConvertUsing(ingr => ingr.IngredientName);
            CreateMap<string, Ingredient>().ConvertUsing(ingredients => new Ingredient() { IngredientName = ingredients });

            CreateMap<CreateRecipeDTO, Recipe>()
            .ForMember(dest => dest.RecipeName, options => options.MapFrom(from => from.RecipeName))
            .ForMember(dest => dest.Ingredients, options => options.MapFrom(src => src.Ingredients));

            CreateMap<Recipe, GetRecipeDTO>()
            .ForMember(dest => dest.RecipeName, options => options.MapFrom(from => from.RecipeName))
            .ForMember(
            dest => dest.Ingredients,
            opt => opt.MapFrom(src => src.Ingredients.Select(i => i.IngredientName
            )));

            CreateMap<Recipe, GetRecipeByIdDTO>().ForMember(dest => dest.RecipeId, opts => opts.MapFrom(s => s.RecipeId))
            .ForMember(dest => dest.RecipeName, opts => opts.MapFrom(s => s.RecipeName))
            .ForMember(dest => dest.Ingredients, opts => opts.MapFrom(s => s.Ingredients));


        }
    }
}