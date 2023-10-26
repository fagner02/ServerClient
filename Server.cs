using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Server<T>
    {
        List<T> Data = new();
        class ServerParams
        {
            public required MethodInfo Method;
        }

        [Request(Port = 2)]
        public void ReadRequest(Socket handler)
        {
            Console.WriteLine("Connected");

            string encodedString = JsonSerializer.Serialize(Data, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });
            handler.Send(Encoding.UTF8.GetBytes(encodedString));
            handler.Close();
            Console.WriteLine("Sent");
        }

        [Request(Port = 1)]
        public void WriteRequest(Socket handler)
        {
            string response = "";
            while (true)
            {
                byte[] buffer = new byte[1024];
                if (handler.Available == 0) break;
                int bytes = handler.Receive(buffer);
                Console.WriteLine(bytes);
                response += Encoding.UTF8.GetString(buffer, 0, bytes);
            }

            List<T>? data = JsonSerializer.Deserialize<List<T>>(response, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });
            if (data == null) return;
            Data = data;
            Console.WriteLine("Pessoas addicionadas: " + data.Count.ToString());
            handler.Send(Encoding.UTF8.GetBytes("Data received"));
            handler.Close();
        }

        public void InstanceEndpoint(object? param)
        {
            ServerParams serverParams = (ServerParams)param!;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ip = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            int port = (int)serverParams.Method.CustomAttributes.First().NamedArguments.First().TypedValue.Value!;
            IPEndPoint localEndPoint = new(IPAddress.Parse(ip), port);
            Console.WriteLine(localEndPoint);

            Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen();

            while (true)
            {
                Socket handler = server.Accept();
                Thread thread = new(() => serverParams.Method.Invoke(this, new object?[] { handler }));
                thread.Start();
            }
        }

        public void Setup()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                if (!method.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(Request))) continue;

                try
                {
                    Thread endpoint = new(new ParameterizedThreadStart(InstanceEndpoint));
                    endpoint.Start(new ServerParams() { Method = method });
                }
                catch (Exception e) { Console.WriteLine("erro"); Console.WriteLine(e); return; }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Request : Attribute
    {
        public int Port;
    }
}