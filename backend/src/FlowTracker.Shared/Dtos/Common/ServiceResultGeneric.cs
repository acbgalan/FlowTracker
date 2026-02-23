using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Dtos.Common
{
    public class ServiceResultGeneric<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}
