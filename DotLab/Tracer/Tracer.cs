using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace DotLab.Tracer
{
    public class Tracer : ITracer
    {
        public Tracer()
        {
            records = new ConcurrentDictionary<int, Stack<TraceRecord>> ();
            res = new TraceResult ();
        }
        public TraceResult GetTraceResult()
        {
            return res;
        }
        private void addRecord(TraceRecord record)
        {
            if(!records.ContainsKey(record.threadID))
                records.TryAdd(record.threadID, new Stack<TraceRecord> ());
            records[record.threadID].Push(record);
        }

        public void StartTrace()
        {
            MethodBase method = new StackTrace().GetFrame(1).GetMethod();
            TraceRecord record = new TraceRecord(method);
            if (!records.ContainsKey(record.threadID) || records[record.threadID].Count < 1)
                res.append(record);
            else
                records[record.threadID].Peek().appendChild(record);
            addRecord(record);
        }

        public void StopTrace()
        {
            int currThreadId = Environment.CurrentManagedThreadId;
            var record = records[currThreadId].Pop();
            record.StopStopwatch();
        }

        ConcurrentDictionary<int, Stack<TraceRecord>> records;
        TraceResult res;
    }
}

