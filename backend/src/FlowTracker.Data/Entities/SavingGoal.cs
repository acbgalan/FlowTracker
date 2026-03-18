using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Entities
{
    public class SavingGoal
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateOnly? Deadline { get; set; }

        public User User { get; set; } = null!;
        public ICollection<SavingLog> SavingLogs { get; set; } = new List<SavingLog>();
    }
}