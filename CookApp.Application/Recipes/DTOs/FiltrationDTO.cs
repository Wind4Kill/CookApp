using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.FiltrationClasses;
namespace CookApp.Application.DTOs.RecipeDTOs
{
    public record FiltrationDTO(string? FiltrationOrder,
    string? FiltrationType,
    string? FiltrationData, int? Page) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FiltrationType is not null && FiltrationData is null)
            {
                yield return new ValidationResult("Filtration type other from default must have filtration value.", [nameof(FiltrationData)]);
            }
            if(Page.HasValue && Page.Value < 1)
            {
                yield return new ValidationResult("Page number can't be less than 1.", new[] { nameof(Page) });
            }
        }
    }
}