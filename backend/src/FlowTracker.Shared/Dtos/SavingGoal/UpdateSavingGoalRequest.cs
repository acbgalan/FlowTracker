using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.SavingGoal
{
    public class UpdateSavingGoalRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public DateOnly? Deadline { get; set; }
        public bool Completed { get; set; }
    }
}
