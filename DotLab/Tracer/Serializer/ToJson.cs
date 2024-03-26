using System.Text.Json;
using DotLab.Tracer.Serializer.DTO;

namespace DotLab.Tracer.Serializer
{
    public class ToJson : ISerializer
    {
        static private JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        public string Serialize(TraceResult result)
        {
            TraceResultBaseDTO dto = new TraceResultBaseDTO(result);
            return JsonSerializer.Serialize(dto, options);
        }
    }
}
