using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Common
{
    public class PagedResponse<T>
    {
        public List<T>? Data { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }
    }
}
