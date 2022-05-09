using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Validations
{
    public class UserModelValidation : AbstractValidator<UserModel>
    {
        public UserModelValidation()
        {
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            RuleFor(u => u.Username).NotEmpty().MinimumLength(6);
            RuleFor(u => u.Email).NotEmpty().EmailAddress(EmailValidationMode.AspNetCoreCompatible);
            RuleFor(u => u.Password).NotEmpty().MinimumLength(8);
            RuleFor(u => u.Firstname).NotEmpty();
            RuleFor(u => u.Lastname).NotEmpty();
        }
    }
}
