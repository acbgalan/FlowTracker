using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Dashboard
{
    public class MonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Expense { get; set; }
        public decimal Income { get; set; }
        public decimal Saving { get; set; }
        public decimal Balance { get; set; }
    }
}
