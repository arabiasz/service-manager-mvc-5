using Inspinia_MVC5_SeedProject.ViewModels.Fiscalizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspinia_MVC5_SeedProject.Models
{
    public class LocalizationValidate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var selectedDevice = (SelectDeviceToFiscalizationViewModel)validationContext.ObjectInstance;

            if ((selectedDevice.Selected == true) && (selectedDevice.AddressId == 0))
                return new ValidationResult("Wybierz miejsce instalacji");

            return ValidationResult.Success;
        }
    }
}