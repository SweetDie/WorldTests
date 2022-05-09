using FluentValidation;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Validations
{
    public class AnswerValidation : AbstractValidator<AnswerModel>
    {
        public AnswerValidation()
        {
            RuleFor(a => a.Name).NotEmpty();
        }
    }
}
