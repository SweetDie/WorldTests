using System;
using System.Collections.Generic;

namespace WorldTests.Primitive.Models
{
    public class TestModel
    {
        public TestModel()
        {
            Questions = new List<QuestionModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<QuestionModel> Questions { get; set; }
    }
}
