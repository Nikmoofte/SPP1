using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace DotLab.Tracer
{
    public class TraceRecord
    {
        internal TraceRecord(MethodBase method, Stopwatch timer)
        {
            threadID = Environment.CurrentManagedThreadId;
            this.method = method;
            this.timer = timer;
            childMethods = new Dictionary<MethodBase, TraceRecord>(); 
        }
        internal TraceRecord(MethodBase method) : this(method, Stopwatch.StartNew()) { }
        internal void StopStopwatch()
        {
            timer.Stop();
        }
        internal void appendChild(TraceRecord record)
        {
            childMethods.Add(record.method, record);
        }
        /*public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in childMethods)
            {
                sb.Append(item.Value.ToString());
            }
            if (sb.Length < 1)
                sb.Append("None");
            return "Traced class " + method.ReflectedType.Name + "\n"
                    + "Traced method " + method.Name + "\n"
                    + "Elapsed time " + timer.ElapsedMilliseconds + " ms. \n"
                    + "Child methods: \n" + sb.ToString();
        }*/
        public bool Equals(TraceRecord other)
        {
            return method.Equals(other.method);
        }
        public bool Equals(MethodBase other)
        {
            return method.Equals(other);
        }
   
        public readonly MethodBase method;
        public readonly int threadID;
        public readonly Stopwatch timer;
        public readonly Dictionary<MethodBase, TraceRecord> childMethods;
    }

    public class TraceResult
    {
        internal TraceResult() 
        { 
            records = new ConcurrentDictionary<int, List<TraceRecord>>();
        }

        internal void append(TraceRecord record) 
        {
            if(!records.ContainsKey(record.threadID))
                records.TryAdd(record.threadID, new List<TraceRecord>());
            records[record.threadID].Add(record);
        }
        internal void appendNested(TraceRecord record, int nestingLevel)
        {
            if (nestingLevel != 1)
            {
                StackTrace stackTrace = new StackTrace();
                MethodBase parent = stackTrace.GetFrame(nestingLevel + 1).GetMethod();
                TraceRecord toAddIn = records[record.threadID].Find(x => x.Equals(parent));
                for (int i = nestingLevel; i > 2; --i)
                {
                    var nextMethod = stackTrace.GetFrame(i).GetMethod();
                    toAddIn = toAddIn.childMethods[nextMethod];
                }
                toAddIn.appendChild(record);
            }
            else
                append(record);
        }

        public override string ToString() 
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in records)
            {
                sb.Append("In " + item.Key + " thread:\n");
                foreach (var record in item.Value)
                    sb.Append(record.ToString());
            }
            return sb.ToString();
        }
        public readonly ConcurrentDictionary<int, List<TraceRecord>> records;
    }
}
