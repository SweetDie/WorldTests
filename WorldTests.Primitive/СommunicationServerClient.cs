using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTests.Primitive
{
    public enum Command
    {
        Login,
        Register,
        LoginError,
        RegisterError,
        GetTests,
        SaveTest
    }

    public class СommunicationServerClient
    {
        public СommunicationServerClient()
        {
        }

        public СommunicationServerClient(Command command, object data)
        {
            Command = command;
            Data = data;
        }

        public Command Command { get; set; }
        public object Data { get; set; }
    }
}
