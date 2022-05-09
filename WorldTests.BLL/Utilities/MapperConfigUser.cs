using AutoMapper;
using WorldTests.DAL.Entities;
using WorldTests.Primitive.Models;

namespace WorldTests.BLL.Utilities
{
    public class MapperConfigUser : Profile
    {
        public MapperConfigUser()
        {
            AllowNullCollections = true;
            CreateMap<User, UserModel>()
                .ForMember(userModel => userModel.Password, user => user.MapFrom(u => u.Credential.Password))
                .ForMember(userModel => userModel.Username, user => user.MapFrom(u => u.Credential.Username));
            CreateMap<UserModel, User>()
                .ForMember(user => user.Credential, userModel => userModel.Ignore());
        }
    }
}
