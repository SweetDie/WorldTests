using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorldTests.Client.Validations
{
    public class EmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString() == string.Empty)
            {
                return new ValidationResult(false, "Must not be empty");
            }

            return ValidationResult.ValidResult;
        }
    }
}
