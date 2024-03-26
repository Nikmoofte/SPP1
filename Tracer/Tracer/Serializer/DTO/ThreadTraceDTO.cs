using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tracer.Serializer.DTO
{
    public class ThreadTraceDTO
    {
        public ThreadTraceDTO(int id)
        {
            this.id = id;
            totalTime = "";
            records = new List<TraceRecordBaseDTO>();
        }
        public ThreadTraceDTO()
        {
            id = 0;
            totalTime = "";
            records = new List<TraceRecordBaseDTO>();
        }
        public int id { get; set; }
        public string totalTime { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public long timeNumb { get; internal set; }
        public List<TraceRecordBaseDTO> records { get; set; }
    }

}
