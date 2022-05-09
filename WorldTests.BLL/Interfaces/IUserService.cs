using System;
using System.Collections.Generic;
using WorldTests.DAL.Entities;
using WorldTests.Primitive.Models;

namespace WorldTests.BLL.Interfaces
{
    public interface IUserService
    {
        void CreateUser(UserModel user);
        void EditUser(UserModel user);
        void RemoveUser(UserModel user);
        UserModel GetUser(Guid id);
        UserModel GetUser(string username);
        ICollection<UserModel> GetUsers(Func<User, bool> predicate);
    }
}
