using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer
{
    interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResult GetTraceResult();
    }
}
