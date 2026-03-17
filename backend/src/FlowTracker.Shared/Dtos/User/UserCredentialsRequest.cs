using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.User
{
    public class UserCredentialsRequest
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
