using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorldTests.BLL.Interfaces;
using WorldTests.Primitive;
using WorldTests.Primitive.Models;

namespace WorldTests.Server
{
    public class ServerCommunication
    {
        private readonly IUserService _userService;
        private readonly ITestService _testService;

        public ServerCommunication(IUserService userService, ITestService testService)
        {
            _userService = userService;
            _testService = testService;
        }

        public void ClientLoginAnswer(Client client, СommunicationServerClient communication)
        {
            var data = JsonSerializer.Deserialize<CredentialModel>(communication.Data.ToString());
            var userModel = _userService.GetUser(data.Username);
            if (userModel == null)
            {
                LoginErrorIncorrectCredentials(client);
            }
            else
            {
                if (userModel.Password == data.Password)
                {
                    var communicationAnswer = new СommunicationServerClient(Command.Login, userModel);
                    var sendData = JsonSerializer.Serialize(communicationAnswer, typeof(СommunicationServerClient));
                    var sendBytes = Encoding.UTF8.GetBytes(sendData);
                    client.NetworkStream.Write(sendBytes);
                }
                else
                {
                    LoginErrorIncorrectCredentials(client);
                }
            }
        }

        public void ClientRegisterAnswer(Client client, СommunicationServerClient communication)
        {
            var data = JsonSerializer.Deserialize<UserModel>(communication.Data.ToString());
            string validationUsernameEmail = UsernameEmailCoincidence(data);
            if (validationUsernameEmail != string.Empty)
            {
                var communicationAnswer = new СommunicationServerClient(Command.RegisterError, validationUsernameEmail);
                var sendData = JsonSerializer.Serialize(communicationAnswer, typeof(СommunicationServerClient));
                var sendBytes = Encoding.UTF8.GetBytes(sendData);
                client.NetworkStream.Write(sendBytes);
            }
            else
            {
                _userService.CreateUser(data);

                var communicationAnswer = new СommunicationServerClient(Command.Register, "done");
                var sendData = JsonSerializer.Serialize(communicationAnswer, typeof(СommunicationServerClient));
                var sendBytes = Encoding.UTF8.GetBytes(sendData);
                client.NetworkStream.Write(sendBytes);
            }
        }

        public void ClientGetTestsAnswer(Client client, СommunicationServerClient communication)
        {
            var tests = _testService.ReadAllTests().ToList();
            var communicationAnswer = new СommunicationServerClient(Command.GetTests, tests);
            var sendData = JsonSerializer.Serialize(communicationAnswer, typeof(СommunicationServerClient));
            var sendBytes = Encoding.UTF8.GetBytes(sendData);
            client.NetworkStream.Write(sendBytes);
        }

        public void SaveTest(Client client, СommunicationServerClient communication)
        {
            var data = JsonSerializer.Deserialize<TestModel>(communication.Data.ToString());
            _testService.CreateTest(data);
        }

        void LoginErrorIncorrectCredentials(Client client)
        {
            var communicationAnswer = new СommunicationServerClient(Command.LoginError, "Incorrect username or password");
            var sendData = JsonSerializer.Serialize(communicationAnswer, typeof(СommunicationServerClient));
            var sendBytes = Encoding.UTF8.GetBytes(sendData);
            client.NetworkStream.Write(sendBytes);
        }

        string UsernameEmailCoincidence(UserModel userModel)
        {
            var user = _userService.GetUser(userModel.Username);
            if (user != null)
            {
                return "This username is already in use";
            }
            user = _userService.GetUsers(u => u.Email == userModel.Email).FirstOrDefault();
            if (user != null)
            {
                return "This email is already in use";
            }
            return string.Empty;
        }
    }
}
