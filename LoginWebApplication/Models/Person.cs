using System;

namespace LoginWebApplication
{
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid Id { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
