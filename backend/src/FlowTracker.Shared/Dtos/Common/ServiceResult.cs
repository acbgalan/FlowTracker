using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
