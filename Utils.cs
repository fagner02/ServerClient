using System.Text.Json;

namespace SD
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Request : Attribute
    {
        public int Port;
    }

    public static class Utils
    {
        public static readonly JsonSerializerOptions JsonOptions = new() { IncludeFields = true, WriteIndented = true };
        public static int GetRequestPort(string name, Type classType)
        {
            return (int)classType.GetMethod(name)!.CustomAttributes.First().NamedArguments.First().TypedValue.Value!;
        }
    }
}