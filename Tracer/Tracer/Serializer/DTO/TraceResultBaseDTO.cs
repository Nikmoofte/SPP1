using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer.Serializer.DTO
{
    public class TraceResultBaseDTO
    {
        public TraceResultBaseDTO(TraceResult tr)
        {
            threads = new List<ThreadTraceDTO>();
            foreach (var item in tr.records)
            {
                appendRecords(item);
            }
        }
        public TraceResultBaseDTO()
        {
            threads = new List<ThreadTraceDTO>();
        }
        void appendRecords(KeyValuePair<int, List<TraceRecord>> item)
        {
            threads.Add(new ThreadTraceDTO(item.Key));
            int ind = threads.Count - 1;
            long totalTime = 0;
            foreach (var record in item.Value)
            {
                threads[ind].records.Add(new TraceRecordBaseDTO(record));
                totalTime += record.timer.ElapsedMilliseconds;
            }
            threads[ind].timeNumb = totalTime;
            threads[ind].totalTime = totalTime + "ms";
        }
        public ThreadTraceDTO? GetThreadTrace(int id)
        {
            return threads.Find(t => t.id == id);
        }

        public List<ThreadTraceDTO> threads { get; private set; }
    }
}
