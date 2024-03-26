using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;

namespace DotLab.Tracer.Serializer.DTO
{
    public class ThreadTrace
    {
        public ThreadTrace(int id)
        {
            this.id = id;
            totalTime = "";
            records = new List<TraceRecordBaseDTO>();
        }
        public ThreadTrace()
        {
            id = 0;
            totalTime = "";
            records = new List<TraceRecordBaseDTO>();
        }
        public int id { get; set; }
        public string totalTime { get; set; }
        public List<TraceRecordBaseDTO> records { get; set; }
    }

}
