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

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();
    }
}
