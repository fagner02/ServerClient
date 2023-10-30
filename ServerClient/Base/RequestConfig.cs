using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace SD
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Request : Attribute
    {
        public int Port;
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SystemServer : Attribute
    {
        public SystemServer(int start, int timeout = -1, int delay = 0) { }
    }

    public static class RequestConfig
    {
        public static readonly JsonSerializerOptions JsonOptions = new() { IncludeFields = true, WriteIndented = true };
        public static int GetRequestPort(string name, Type serverType, Type systemType)
        {
            int portStart = 0;
            if (systemType != null) portStart = GetPortStart(serverType, systemType);
            return GetRequestPort(name, serverType) + portStart;
        }

        public static int GetRequestPort(string name, Type serverType, int portStart)
        {
            return GetRequestPort(name, serverType) + portStart;
        }

        public static int GetRequestPort(string name, Type serverType)
        {
            return (int)serverType.GetMethod(name)!.CustomAttributes.First().NamedArguments.First().TypedValue.Value!;
        }

        private static int GetPortStart(Type server, Type system)
        {
            return (int)system.GetFields().FirstOrDefault(x => x.FieldType == server)!.CustomAttributes.First().ConstructorArguments[0].Value!;
        }
        public static bool IsSystemClass(Type classType)
        {
            return classType == typeof(SystemBase);
        }
        public static void ResolveRequestMethods(Action<MethodInfo> callback, Type type)
        {

            var methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (!method.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(Request))) continue;
                callback(method);
            }
        }
    }
}