using FlowTracker.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public TransactionType Type { get; set; }
        public string? Icon { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? UserId { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public IdentityUser? User { get; set; }
    }
}
