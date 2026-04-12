using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Transaction
{
    public class TransactionResponse
    {
        public int Id { get; set; }
        public required decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public required string CategoryName { get; set; }
        public required string Type { get; set; }
        public required string Icon { get; set; }
        public required string CategoryDescription { get; set; }
        public required string UserId { get; set; }
    }
}