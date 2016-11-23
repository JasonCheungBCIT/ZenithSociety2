using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenithWebsite.Models.CustomValidation
{
    public class AfterNow : ValidationAttribute
    {
        public AfterNow() : base("{0} must be after the current time") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime datetime = (DateTime)value;

            if (datetime < DateTime.Now)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
