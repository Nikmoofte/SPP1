namespace DotLab.Tracer.Serializer
{
    internal interface ISerializer
    {
         string Serialize(TraceResult result);
    }
}
