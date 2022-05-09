using AutoMapper;
using WorldTests.DAL.Entities;
using WorldTests.Primitive.Models;

namespace WorldTests.BLL.Utilities
{
    public class MapperConfigTest : Profile
    {
        public MapperConfigTest()
        {
            AllowNullCollections = true;
            CreateMap<Test, TestModel>();
            CreateMap<Question, QuestionModel>();
            CreateMap<Answer, AnswerModel>();

            CreateMap<TestModel, Test>();
            CreateMap<QuestionModel, Question>()
                .ForMember(question => question.Test, questionModel => questionModel.Ignore());
            CreateMap<AnswerModel, Answer>()
                .ForMember(answer => answer.Question, answerModel => answerModel.Ignore());
        }
    }
}
