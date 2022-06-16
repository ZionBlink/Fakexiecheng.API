using Fakexiecheng.API.validationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fakexiecheng.API.Dtos
{
    public class TouristRouteForCreationDto :TouristRouteForManipulationDto//:IValidatableObject
    {


     /*   public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description) 
            
            {
                yield return new ValidationResult("路线名称必须与路线描述不同",
                    new[] {"TourisRouteForCreationDto" });
            
            
            }


        }*/
    }
}
