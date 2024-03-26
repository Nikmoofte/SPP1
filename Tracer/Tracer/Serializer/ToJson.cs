using System.Text.Json;
using Tracer.Serializer.DTO;

namespace Tracer.Serializer
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
