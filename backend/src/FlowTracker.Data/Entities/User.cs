using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Entities
{
    public sealed class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
