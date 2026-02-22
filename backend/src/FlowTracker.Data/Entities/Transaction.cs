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
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}