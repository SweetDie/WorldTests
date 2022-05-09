using FluentValidation;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Validations
{
    public class QuestionValidation : AbstractValidator<QuestionModel>
    {
        public QuestionValidation()
        {
            RuleFor(q => q.Name).NotEmpty();
        }
    }
}
