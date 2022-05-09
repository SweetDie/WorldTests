using AutoMapper;
using System;
using System.Collections.Generic;
using WorldTests.BLL.Interfaces;
using WorldTests.DAL.Entities;
using WorldTests.DAL.Interfaces;
using WorldTests.Primitive.Models;

namespace WorldTests.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IDataGenericRepository<User> _userRepo;
        private readonly IDataGenericRepository<Credential> _credentialRepo;
        private readonly Mapper _userMapper;

        public UserService(IDataGenericRepository<User> userRepo, IDataGenericRepository<Credential> credentialRepo, Mapper userMapper)
        {
            this._userRepo = userRepo;
            this._credentialRepo = credentialRepo;
            this._userMapper = userMapper;
        }

        public void CreateUser(UserModel userModel)
        {
            var crypto = new SimpleCrypto.PBKDF2();

            var credential = new Credential
            {
                Id = Guid.NewGuid(),
                Password = userModel.Password,
                Username = userModel.Username,
                Salt = crypto.GenerateSalt()
            };
            userModel.CredentialId = credential.Id;
            var user = _userMapper.Map<User>(userModel);
            user.Credential = credential;
            _userRepo.Create(user);
        }

        public void EditUser(UserModel userModel)
        {
            var user = _userMapper.Map<User>(userModel);
            _userRepo.Edit(user);
        }

        public void RemoveUser(UserModel userModel)
        {
            var user = _userMapper.Map<User>(userModel);
            _userRepo.Remove(user);
            var credential = _credentialRepo.Get(user.CredentialId);
            _credentialRepo.Remove(credential);
        }

        public UserModel GetUser(Guid id)
        {
            var user = _userRepo.Get(id);
            var credential = _credentialRepo.Get(user.CredentialId);
            user.Credential = credential;
            return _userMapper.Map<UserModel>(user);
        }

        public UserModel GetUser(string username)
        {
            var credential = _credentialRepo.Get(x => x.Username == username);
            if (credential == null)
            {
                return null;
            }
            var user = _userRepo.Get(x => x.CredentialId == credential.Id);
            user.Credential = credential;
            return _userMapper.Map<UserModel>(user);
        }

        public ICollection<UserModel> GetUsers(Func<User, bool> predicate)
        {
            var users = _userRepo.GetAll(predicate);
            return _userMapper.Map<ICollection<UserModel>>(users);
        }
    }
}
