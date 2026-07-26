using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Model.FiltrationClasses;

namespace CookApp.Model.DTOs
{
    public record FiltrationDTO(string? FiltrationOrder,
    string? FiltrationType,
    string? FiltrationData, int? Page) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Enum.TryParse<FiltrationFilter>(FiltrationType, out FiltrationFilter result))
            {
                if (result != FiltrationFilter.Default && FiltrationData is null)
                {
                    yield return new ValidationResult("Filtration type other from default must have filtration value.", [nameof(FiltrationData)]);
                }
            }
        }
    }
}