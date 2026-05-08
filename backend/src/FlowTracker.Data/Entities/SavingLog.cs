using System;
using System.Collections.Generic;
using System.Text;
using FlowTracker.Shared.Enums;

namespace FlowTracker.Data.Entities
{
    public class SavingLog
    {
        public int Id { get; set; }
        public int SavingGoalId { get; set; }
        public decimal Amount { get; set; }
        public MovementType Type { get; set; }
        public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow;
        public int TransactionId { get; set; }

        public SavingGoal SavingGoal { get; set; } = null!;
        public Transaction Transaction { get; set; } = null!;
    }
}