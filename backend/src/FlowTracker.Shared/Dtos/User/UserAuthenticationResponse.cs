using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.User
{
    public class UserAuthenticationResponse
    {
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
