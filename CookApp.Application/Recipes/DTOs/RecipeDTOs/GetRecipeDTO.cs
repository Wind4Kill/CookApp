using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CookApp.Model.Entities;


namespace CookApp.Application.DTOs.RecipeDTOs
{
    public class GetRecipeDTO
    {
        public string RecipeName { get; set; } = null!;
        public string[] Ingredients { get; set; } = null!;
    }
}