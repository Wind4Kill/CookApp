using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.DTOs
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