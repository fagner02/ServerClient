using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SD
{
    public class Client
    {
        public Type ServerType;
        public Type? SystemType;
        public Client(Type serverType, Type systemType)
        {
            ServerType = serverType;
            SystemType = systemType;
        }
        public Client(Type serverType)
        {
            ServerType = serverType;
        }
        public void MakeRequest(string method, string? message = null)
        {
            int port;

            if (SystemType == null)
                port = RequestConfig.GetRequestPort(method, ServerType);
            else
                port = RequestConfig.GetRequestPort(method, ServerType, SystemType);

            InternMakeRequest(port, message);
        }

        private static void InternMakeRequest(int port, string? message)
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse("192.168.100.95"), port);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndPoint);

            if (message != null)
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                int bytes = client.Send(messageBytes);
            }

            string response = "";
            while (true)
            {
                byte[] buffer = new byte[1024];
                int resBytes = client.Receive(buffer);
                if (resBytes == 0) break;
                response += Encoding.UTF8.GetString(buffer, 0, resBytes);
            }

            Console.WriteLine(response);
            client.Close();
        }

        public void MakeRequestUdp(string method, string? message = null)
        {
            // int port = RequestConfig.GetRequestPort(method, typeof(Udp<Pessoa>));
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ip = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            EndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 1);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), IPAddress.Parse(ip));
            client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            client.Bind(ipEndPoint);

            string response = "";
            while (true)
            {
                byte[] buffer = new byte[1024];
                int resBytes = client.ReceiveFrom(buffer, ref remoteIp);
                if (resBytes == 0) break;
                response += Encoding.UTF8.GetString(buffer, 0, resBytes);
            }

            Console.WriteLine(response);
            client.Close();
            return;
        }
    }
}