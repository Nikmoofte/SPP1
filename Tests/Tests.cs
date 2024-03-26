using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Diagnostics;
using Tracer;
using Tracer.Serializer;
using Tracer.Serializer.DTO;

namespace Tests
{

    [TestFixture]
    public class TracerLibTests
    {
        public ITracer Tracer = new Tracer.Tracer();

        private readonly List<Thread> _threads = new List<Thread>();

        readonly int ThreadsCount = 10;
        readonly int MethodsCount = 15;

        readonly int MillisecondsTimeout = 100;

        private void Method()
        {
            Tracer.StartTrace();
            Thread.Sleep(MillisecondsTimeout);
            Tracer.StopTrace();
        }

        // TIME
        [Test]
        public void ExecutionTimeMoreThreadTimeout()
        {
            Method();
            TraceResult traceResult = Tracer.GetTraceResult();

            var res = new TraceResultBaseDTO(traceResult);
            long methodTime = res.GetThreadTrace(Thread.CurrentThread.ManagedThreadId).timeNumb;
            double threadTime = res.GetThreadTrace(Thread.CurrentThread.ManagedThreadId).timeNumb;


            Assert.IsTrue(methodTime >= MillisecondsTimeout);
            Assert.IsTrue(threadTime >= MillisecondsTimeout);
        }

        [Test]
        public void ThreadTimeIsCorrect()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Method();
            Method();
            Method();

            var time = stopwatch.ElapsedMilliseconds;

            TraceResult traceResult = Tracer.GetTraceResult();
            var res = new TraceResultBaseDTO(traceResult);

            double threadTime = res.GetThreadTrace(Thread.CurrentThread.ManagedThreadId).timeNumb;

            bool flag = threadTime + 5 >= time;
            flag |= threadTime - 5 <= time;

            Assert.IsTrue(flag);
        }

        [Test]
        public void MethodTimeIsCorrect()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Method();
            Method();
            Method();
            var time = stopwatch.ElapsedMilliseconds;


            TraceResult traceResult = Tracer.GetTraceResult();
            var res = new TraceResultBaseDTO(traceResult);

            var methodTime = res.GetThreadTrace(Thread.CurrentThread.ManagedThreadId).records[0].timeNumb;

            Console.WriteLine(time);
            Console.WriteLine(methodTime);

            bool flag = methodTime + 5 >= time;
            flag |= methodTime - 5 <= time;

            Assert.IsTrue(flag);
        }

        // THREADS
        [Test]
        public void ThreadCount()
        {
            for (int i = 0; i < ThreadsCount - 1; i++)
            {
                _threads.Add(new Thread(Method));
            }

            foreach (Thread thread in _threads)
            {
                thread.Start();
                thread.Join();
            }

            TraceResult traceResult = Tracer.GetTraceResult();
            var res = new TraceResultBaseDTO(traceResult);
            ToJson toJson = new ToJson();
            Assert.AreEqual(ThreadsCount, res.threads.Count);
        }

        // METHODS
        [Test]
        public void MethodCount()
        {
            for (int i = 0; i < MethodsCount - 1; i++)
            {
                Method();
            }

            TraceResult traceResult = Tracer.GetTraceResult();
            var res = new TraceResultBaseDTO(traceResult);
            Assert.AreEqual(MethodsCount, res.GetThreadTrace(Thread.CurrentThread.ManagedThreadId).records.Count);
        }
    }
 

}
