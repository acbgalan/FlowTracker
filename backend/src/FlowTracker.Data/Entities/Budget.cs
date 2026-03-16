using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public required string UserId { get; set; }
        public decimal LimitAmount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public Category Category { get; set; } = null!;
        public IdentityUser IdentityUser { get; set; } = null!;
    }
}
