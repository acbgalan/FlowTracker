using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.SavingGoal
{
    public class SavingGoalResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateOnly? Deadline { get; set; }
        public decimal ProgressPercentaje { get; set; }
    }
}
