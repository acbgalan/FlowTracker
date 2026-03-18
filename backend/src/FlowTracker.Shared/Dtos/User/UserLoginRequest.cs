using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.User
{
    public class UserLoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
