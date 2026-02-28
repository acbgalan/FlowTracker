using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Category
{
    public class UpdateCategoryRequest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public TransactionType Type { get; set; }
        public string? Icon { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
