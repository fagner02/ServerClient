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

        public void InstanceEndpoint()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string localIp = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            IPEndPoint localEndPoint = new(IPAddress.Parse(localIp), 0);
            Console.WriteLine(localEndPoint);

            Socket server = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(localEndPoint);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), IPAddress.Parse(localIp));
            server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            Console.WriteLine(multicastOption);
            server.SendTo(Encoding.UTF8.GetBytes("hamina"), new IPEndPoint(IPAddress.Parse("224.168.100.2"), 1));
        }

        public void Setup(int portStart = 0)
        {
            InstanceEndpoint();
        }
    }
}