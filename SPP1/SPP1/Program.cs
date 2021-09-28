using System;
using System.Threading;
using Tracer;

namespace SPP1
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracer tracer = new Tracer();

            Foo _foo = new Foo(tracer);
            Bar _bar = new Bar(tracer);
            FirstTestClass _anotherObject = new FirstTestClass(tracer);

            tracer.StartTrace();
            _anotherObject.FirstTestMethod();
            _bar.InnerMethod();
            tracer.StopTrace();

            Thread secondThread = new Thread(new ThreadStart(_foo.MyMethod));
            secondThread.Start();

            Thread thirdThread = new Thread(new ThreadStart(_bar.InnerMethod));
            thirdThread.Start();

            secondThread.Join();
            thirdThread.Join();

            TraceResult traceResult = tracer.GetTraceResult();
        }
    }
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }
        public void MyMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _bar.InnerMethod();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
    }

    public class FirstTestClass
    {
        private ITracer _tracer;
        private Bar _bar;
        private int n = 3;

        internal FirstTestClass(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void FirstTestMethod()
        {
            _tracer.StartTrace();
            while (n != 0)
            {
                n--;
                FirstTestMethod();
                _bar.InnerMethod();
            }
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
    }
}
