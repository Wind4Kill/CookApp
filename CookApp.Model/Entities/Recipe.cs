using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Model.DTOs;
using CookApp.Model.Entities;

namespace CookApp.Model;

[AutoMap(typeof(CreateRecipeDTO))]
public class Recipe
{
    public DateOnly CreatedAt { get; private set; }

    public int RecipeId { get; set; }

    public string RecipeName { get; set; } = null!;

    public List<Ingredient> Ingredients { get; set; } = null!;

    public bool IsDeleted { get; set; }

}