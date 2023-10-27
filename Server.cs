using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Server<T>
    {
        protected List<T> Data = new();
        class ServerParams
        {
            public required MethodInfo Method;
        }

        [Request(Port = 2)]
        public virtual void ReadRequest(Socket handler, CancellationToken cancellationToken)
        {
            Console.WriteLine("Connected");

            string encodedString = JsonSerializer.Serialize(Data, Utils.JsonOptions);
            if (cancellationToken.IsCancellationRequested) return;
            handler.Send(Encoding.UTF8.GetBytes(encodedString));
            handler.Close();
            Console.WriteLine("Sent");
        }

        [Request(Port = 1)]
        public virtual void WriteRequest(Socket handler, CancellationToken cancellationToken)
        {
            Console.WriteLine("in");

            string response = "";
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) return;
                byte[] buffer = new byte[1024];
                if (handler.Available == 0) break;
                int bytes = handler.Receive(buffer);
                Console.WriteLine(bytes);
                response += Encoding.UTF8.GetString(buffer, 0, bytes);
            }

            List<T>? data = JsonSerializer.Deserialize<List<T>>(response, Utils.JsonOptions);
            if (data == null) return;
            if (cancellationToken.IsCancellationRequested) return;
            Data = data;
            Console.WriteLine("Pessoas adicionadas: " + data.Count.ToString());
            handler.Send(Encoding.UTF8.GetBytes("Data received"));
            handler.Close();
        }

        public void InstanceEndpoint(object? param)
        {
            ServerParams serverParams = (ServerParams)param!;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ip = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            int port = Utils.GetRequestPort(serverParams.Method.Name, this.GetType());
            IPEndPoint localEndPoint = new(IPAddress.Parse(ip), port);
            Console.WriteLine(localEndPoint);

            Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(localEndPoint);
            server.Listen();

            CancellationTokenSource cts = new(50000);
            HandleConnect(server, serverParams.Method, cts.Token);
        }

        public void HandleConnect(Socket server, MethodInfo method, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Timeout");
                    return;
                }
                try
                {
                    var res = server.AcceptAsync(cancellationToken).AsTask();
                    res.Wait(cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Timeout");
                        return;
                    }
                    if (!res.IsCompletedSuccessfully) return;

                    Socket handler = res.Result;
                    Thread thread = new(() => method.Invoke(this, new object?[] { handler, cancellationToken }));
                    thread.Start();
                }
                catch
                {
                    Console.WriteLine("Timeout");
                    return;
                }
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
}