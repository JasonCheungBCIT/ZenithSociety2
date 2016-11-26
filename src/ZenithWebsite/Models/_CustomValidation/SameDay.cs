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
    public class SameDay : ValidationAttribute
    {
        private string otherDatePropertyName;

        public SameDay(string otherDatePropertyName) : base("Must be on the same day as " + otherDatePropertyName)
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

            if (thisDate.Date != otherDate.Date)
            { 
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
