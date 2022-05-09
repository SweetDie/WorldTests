using FluentValidation;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Validations
{
    public class TestValidation : AbstractValidator<TestModel>
    {
        public TestValidation()
        {
            RuleFor(t => t.Name).NotEmpty();
        }
    }
}
