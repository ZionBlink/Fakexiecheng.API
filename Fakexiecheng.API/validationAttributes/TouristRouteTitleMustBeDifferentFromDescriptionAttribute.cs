using Fakexiecheng.API.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fakexiecheng.API.validationAttributes
{
    public class TouristRouteTitleMustBeDifferentFromDescriptionAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForManipulationDto)validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)

            {
                 return new ValidationResult("路线名称必须与路线描述不同",
                    new[] { "TourisRouteForCreationDto" });


            }
            return ValidationResult.Success;

        }


    }
}
