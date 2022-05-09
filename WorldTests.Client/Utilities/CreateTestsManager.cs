using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Utilities
{
    public class CreateTestsManager
    {
        public TestModel TestModel { get; set; }

        public CreateTestsManager()
        {
            TestModel = new TestModel { Id = Guid.NewGuid() };
        }

        public void AddQuestion(QuestionModel questionModel)
        {
            TestModel.Questions.Add(questionModel);
        }
    }
}
