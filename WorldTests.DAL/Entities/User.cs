using System;

namespace WorldTests.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Image { get; set; }

        public Guid CredentialId { get; set; }
        public Credential Credential { get; set; }
    }
}
