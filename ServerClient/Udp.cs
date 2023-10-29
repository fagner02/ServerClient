using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Udp<T>
    {
        protected List<T> Data = new();
        public int Timeout = -1;
        class ServerParams
        {
            public int PortStart = 0;
            public required MethodInfo Method;
        }

        [Request(Port = 2)]
        public virtual void ReadRequest(Socket handler, CancellationToken cancellationToken)
        {
            Console.WriteLine("Connected");

            string encodedString = JsonSerializer.Serialize(Data, RequestConfig.JsonOptions);
            if (cancellationToken.IsCancellationRequested) return;
            handler.Send(Encoding.UTF8.GetBytes(encodedString));
            handler.Close();
            Console.WriteLine("Sent");
        }

        public void InstanceEndpoint(object? param)
        {
            ServerParams serverParams = (ServerParams)param!;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ip = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            // int port = RequestConfig.GetRequestPort(serverParams.Method.Name, this.GetType());
            IPEndPoint localEndPoint = new(IPAddress.Parse(ip), 0);
            Console.WriteLine(localEndPoint);

            Socket server = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(localEndPoint);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), IPAddress.Parse(ip));
            server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

            server.SendTo(Encoding.UTF8.GetBytes("hamina"), new IPEndPoint(IPAddress.Parse("224.168.100.2"), 1));
        }

        public void Setup(int portStart = 0)
        {
            InstanceEndpoint(new ServerParams() { Method = GetType().GetMethod(nameof(ReadRequest)), PortStart = portStart });
        }
    }
}