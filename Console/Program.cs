
using System;
using System.Threading;
using System.Threading.Tasks;
using Tracer;


internal class Program
{
    static ITracer tracer = new Tracer.Tracer();
    static void Main(string[] args)
    {
        toTest();
        toTest2();
        toTest();
        Task task = new Task(toTest2);
        task.Start();
        task.Wait();
        var result = tracer.GetTraceResult();
        Tracer.Serializer.ToXML toXML = new Tracer.Serializer.ToXML();
        Console.WriteLine(toXML.Serialize(result));
        Console.WriteLine();
        Tracer.Serializer.ToJson toJson = new Tracer.Serializer.ToJson();
        Console.WriteLine(toJson.Serialize(result));
        Console.WriteLine();
    }

    static void toTest()
    {
        tracer.StartTrace();

        Thread.Sleep(800);
        toTest2();
        toTest3();

        tracer.StopTrace();

    }

    static void toTest2()
    {
        tracer.StartTrace();

        Thread.Sleep(800);

        tracer.StopTrace();
    }
    static void toTest3()
    {
        tracer.StartTrace();

        Thread.Sleep(400);

        tracer.StopTrace();
    }
}