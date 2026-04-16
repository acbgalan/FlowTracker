using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.SavingLog
{
    public class SavingLogResponse
    {
        public int Id { get; set; }
        public int SavingGoalId { get; set; }
        public decimal Amount { get; set; }
        public required string Type { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
