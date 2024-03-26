using System;
using System.IO;
using System.Xml.Serialization;


namespace DotLab.Tracer.Serializer
{
    public class ToXML : ISerializer
    {
        static private Type[] arr = new Type[2] { typeof(DTO.ThreadTrace), typeof(DTO.TraceRecordBaseDTO) };
        public string Serialize(TraceResult result)
        {
            var dto = new DTO.TraceResultBaseDTO(result);
            XmlSerializer serializer = new XmlSerializer(dto.GetType(), arr);
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, dto);
                return textWriter.ToString();
            }
        }
    }
}
