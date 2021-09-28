using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Collections.Concurrent;

namespace Tracer
{
    public class TracerClass : ITracer
    {
        private TraceResult traceResult = new TraceResult();
        private ConcurrentDictionary<int, Stack<(Methods, Stopwatch)>> dictionaryOfThreads = new ConcurrentDictionary<int, Stack<(Methods, Stopwatch)>>();

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
           // Console.WriteLine("Test");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            StackFrame frame = new StackFrame(1);
            MethodBase frameMethod = frame.GetMethod();

            Methods method = new Methods();
            method.Class = frameMethod.DeclaringType.Name;
            method.Name = frameMethod.Name;

            int ThreadId = Thread.CurrentThread.ManagedThreadId;

            if (dictionaryOfThreads.TryAdd(ThreadId, new Stack<(Methods, Stopwatch)>()))
            {
                traceResult.Threads.Add(new Threads { id = ThreadId });
            }

            dictionaryOfThreads[ThreadId].Push((method, stopwatch));
        }

        public void StopTrace()
        {
            // Console.WriteLine("Test");
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            (Methods ThisMethod, Stopwatch stopwatch) = dictionaryOfThreads[ThreadId].Pop();
            stopwatch.Stop();
            ThisMethod.Time = stopwatch.ElapsedMilliseconds;

            if (dictionaryOfThreads[ThreadId].Count != 0)
            {
                (Methods PreMethod, Stopwatch preStopwatch) = dictionaryOfThreads[ThreadId].Peek();
                if (PreMethod.Methods == null)
                {
                    PreMethod.Methods = new List<Methods>();
                }
                PreMethod.Methods.Add(ThisMethod);
                // Console.WriteLine("Test");
            }
            else
            {
                int ThreadIndex = traceResult.Threads.FindIndex(_thread => _thread.id == ThreadId);
                if (traceResult.Threads[ThreadIndex].Methods == null)
                {
                    traceResult.Threads[ThreadIndex].Methods = new List<Methods>();
                }
                traceResult.Threads[ThreadIndex].Methods.Add(ThisMethod);
                traceResult.Threads[ThreadIndex].Time += ThisMethod.Time;
                // Console.WriteLine("Test");
            }
        }
    }
}
 
