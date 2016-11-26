using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZenithWebsite.Models.ZenithSocietyModels;

namespace ZenithWebsite.Models.CustomValidation
{
    public class AfterTime : ValidationAttribute
    {
        private string otherDatePropertyName;

        public AfterTime(string otherDatePropertyName) : base("Must be after " + otherDatePropertyName + " time.")
        {
            this.otherDatePropertyName = otherDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (Event)validationContext.ObjectInstance;

            var property = validationContext.ObjectType.GetProperty(otherDatePropertyName);

            if (property == null)
            {
                return new ValidationResult("Unknown property");
            }

            var otherDate = (DateTime)property.GetValue(validationContext.ObjectInstance, null);
            var thisDate = (DateTime)value;

            if (thisDate <= otherDate)
            { 
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
