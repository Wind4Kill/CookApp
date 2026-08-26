using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CookApp.Model.Entities;

namespace CookApp.Application.DTOs.RecipeDTOs
{
    public class CreateRecipeDTO
    {
        [Required]
        [StringLength(150)]
        public string RecipeName { get; set; } = null!;

        [Required]
        public string[] Ingredients { get; set; } = null!;
    }
}