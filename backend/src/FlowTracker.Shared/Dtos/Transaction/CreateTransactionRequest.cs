using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Transaction
{
    public class CreateTransactionRequest
    {
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int? SavingGoalId { get; set; }
    }
}
