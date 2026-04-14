using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.SavingLog
{
    public class CreateSavingLogRequest
    {
        public int SavingGoalId { get; set; }
        public decimal Amount { get; set; }
        public MovementType Type { get; set; }
    }
}
