using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tracer.Serializer.DTO
{
    public class TraceRecordBaseDTO
    {
        public TraceRecordBaseDTO(TraceRecord tr)
        {
            childs = new List<TraceRecordBaseDTO>();
            className = tr.method.ReflectedType.Name;
            methodName = tr.method.Name;
            timeNumb = tr.timer.ElapsedMilliseconds;
            time = timeNumb + "ms";
            foreach (var item in tr.childMethods)
                appendChild(item);
        }
        public TraceRecordBaseDTO()
        {
            childs = new List<TraceRecordBaseDTO>();
            className = "";
            methodName = "";
            time = "0ms";
            timeNumb = 0;
        }
        void appendChild(KeyValuePair<MethodBase, TraceRecord> item)
        {
            childs.Add(new TraceRecordBaseDTO(item.Value));
        }

        public string className { get; set; }
        public string methodName { get; set; }
        public string time { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public readonly long timeNumb;
        public List<TraceRecordBaseDTO> childs { get; set; }
    }
}
