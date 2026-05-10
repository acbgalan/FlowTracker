using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Dashboard
{
    public class MonthlyCategoryExpenseSummary
    {
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
    }
}
