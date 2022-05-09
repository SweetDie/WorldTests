using System;
using System.Collections.Generic;

namespace WorldTests.Primitive.Models
{
    public class QuestionModel
    {
        public QuestionModel()
        {
            Answers = new List<AnswerModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TestId { get; set; }
        public virtual ICollection<AnswerModel> Answers { get; set; }
    }
}
