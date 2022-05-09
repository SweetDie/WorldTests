using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WorldTests.BLL.Interfaces;
using WorldTests.DAL.Entities;
using WorldTests.DAL.Interfaces;
using WorldTests.Primitive.Models;

namespace WorldTests.BLL.Services
{
    public class TestService : ITestService
    {
        private readonly IDataGenericRepository<Test> _testRepo;
        private readonly IDataGenericRepository<Answer> _answerRepo;
        private readonly IDataGenericRepository<Question> _questionRepo;
        private readonly Mapper _testMapper;

        public TestService(IDataGenericRepository<Test> testRepo, IDataGenericRepository<Answer> answerRepo,
            IDataGenericRepository<Question> questionRepo, Mapper testMapper)
        {
            this._testRepo = testRepo;
            this._answerRepo = answerRepo;
            this._questionRepo = questionRepo;
            this._testMapper = testMapper;
        }

        public void CreateTest(TestModel testModel)
        {
            var test = _testMapper.Map<Test>(testModel);
            _testRepo.Create(test);

            foreach (var questionModel in testModel.Questions)
            {
                CreateQuestion(questionModel);
            }
        }

        public void CreateAnswer(AnswerModel answerModel)
        {
            var answer = _testMapper.Map<Answer>(answerModel);
            _answerRepo.Create(answer);
        }

        public void CreateQuestion(QuestionModel questionModel)
        {
            var question = _testMapper.Map<Question>(questionModel);
            _questionRepo.Create(question);

            foreach (var answer in questionModel.Answers)
            {
                CreateAnswer(answer);
            }
        }

        public ICollection<TestModel> ReadAllTests()
        {
            var tests = GetTests();
            foreach (var test in tests)
            {
                test.Questions = GetQuestions(x => x.TestId == test.Id);
                foreach (var question in test.Questions)
                {
                    question.Answers = GetAnswers(x => x.QuestionId == question.Id);
                }
            }
            return tests;
        }

        public ICollection<TestModel> GetTests()
        {
            return GetTests(x => true);
        }

        public ICollection<TestModel> GetTests(Func<Test, bool> predicate)
        {
            var testCollection = _testMapper.Map<ICollection<TestModel>>(_testRepo.GetAll(predicate));
            return testCollection;
        }

        public ICollection<QuestionModel> GetQuestions()
        {
            return GetQuestions(x => true);
        }

        public ICollection<QuestionModel> GetQuestions(Func<Question, bool> predicate)
        {
            var questionCollection = _testMapper.Map<ICollection<QuestionModel>>(_questionRepo.GetAll(predicate));
            return questionCollection;
        }

        public ICollection<AnswerModel> GetAnswers()
        {
            return GetAnswers(x => true);
        }

        public ICollection<AnswerModel> GetAnswers(Func<Answer, bool> predicate)
        {
            var answerCollection = _testMapper.Map<ICollection<AnswerModel>>(_answerRepo.GetAll(predicate));
            return answerCollection;
        }

        public void RemoveTest(TestModel testModel)
        {
            var test = _testMapper.Map<Test>(testModel);

            foreach (var question in test.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    _answerRepo.Remove(answer);
                }
                _questionRepo.Remove(question);
            }
            _testRepo.Remove(test);
        }
    }
}
