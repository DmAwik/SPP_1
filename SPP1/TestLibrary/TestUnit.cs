using NUnit.Framework;
using Tracer;
using System.Diagnostics;
using System.Threading;
using System;

namespace TestLibrary
{
    public class Tests
    {
        TraceResult traceResult;

        [SetUp]
        public void Setup()
        {
            TracerClass tracer = new TracerClass();

            Foo _foo = new Foo(tracer);
            Bar _bar = new Bar(tracer);
            FirstTestClass _anotherObject = new FirstTestClass(tracer);

            _anotherObject.FirstTestMethod();

            Thread secondThread = new Thread(new ThreadStart(_foo.MyMethod));
            secondThread.Start();

            Thread thirdThread = new Thread(new ThreadStart(_bar.InnerMethod));
            thirdThread.Start();

            secondThread.Join();
            thirdThread.Join();

            traceResult = tracer.GetTraceResult();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        [Test]
        public void Test_ThreadCount_3()
        {
            Assert.AreEqual(3, traceResult.Threads.Count);
        }

        
        [Test]
        public void Test_MethodInfo_Bar_InnerMethod()
        {
            Assert.AreEqual("InnerMethod", traceResult.Threads[0].Methods[0].Methods[1].Name, "Wrong method name");
            Assert.AreEqual("Bar", traceResult.Threads[0].Methods[0].Methods[1].Class, "Wrong class name");
        }
        [Test]
        public void TestExecutionTime()
        {
            Stopwatch stopwatch = new Stopwatch();
            TracerClass tracer = new TracerClass();

            stopwatch.Start();
            tracer.StartTrace();

            Thread.Sleep(1000);

            stopwatch.Stop();
            tracer.StopTrace();

            Assert.IsTrue(Math.Abs(stopwatch.ElapsedMilliseconds - tracer.GetTraceResult().Threads[0].Time) < 10);
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