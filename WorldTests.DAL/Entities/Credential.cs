using System;

namespace WorldTests.DAL.Entities
{
    public class Credential
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public User User { get; set; }
    }
}
