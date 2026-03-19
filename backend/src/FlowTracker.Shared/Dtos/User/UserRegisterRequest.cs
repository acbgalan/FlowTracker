using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.User
{
    public class UserRegisterRequest : UserLoginRequest
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
