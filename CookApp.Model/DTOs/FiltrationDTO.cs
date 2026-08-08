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
            if ((FiltrationType is not null||FiltrationType!=FiltrationFilter.Default.ToString())&&FiltrationData is null)
            {
                    yield return new ValidationResult("Filtration type other from default must have filtration value.", [nameof(FiltrationData)]);
            }
        }
    }
}