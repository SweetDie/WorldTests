using System;

namespace WorldTests.Primitive.Models
{
    public class AnswerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
    }
}
