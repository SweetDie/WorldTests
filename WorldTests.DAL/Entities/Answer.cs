using System;

namespace WorldTests.DAL.Entities
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
