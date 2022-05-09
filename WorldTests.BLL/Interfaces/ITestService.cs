using System;
using System.Collections.Generic;
using WorldTests.DAL.Entities;
using WorldTests.Primitive.Models;

namespace WorldTests.BLL.Interfaces
{
    public interface ITestService
    {
        void CreateTest(TestModel testModel);
        void CreateAnswer(AnswerModel answerModel);
        void CreateQuestion(QuestionModel questionModel);
        ICollection<TestModel> ReadAllTests();
        ICollection<TestModel> GetTests();
        ICollection<TestModel> GetTests(Func<Test, bool> predicate);
        ICollection<QuestionModel> GetQuestions();
        ICollection<QuestionModel> GetQuestions(Func<Question, bool> predicate);
        ICollection<AnswerModel> GetAnswers();
        ICollection<AnswerModel> GetAnswers(Func<Answer, bool> predicate);
        void RemoveTest(TestModel testModel);
        //void EditTest(TestModel testModel);
        //void EditAnswer(AnswerModel answerModel);
        //void EditQuestion(QuestionModel questionModel);
        //void RemoveAnswer(AnswerModel answerModel);
    }
}
