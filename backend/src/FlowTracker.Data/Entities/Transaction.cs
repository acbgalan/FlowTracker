using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public required decimal Amount { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public required string UserId { get; set; }

        public Category Category { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<SavingLog> SavingLogs { get; set; } = new List<SavingLog>();
    }
}