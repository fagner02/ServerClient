using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public class MulticastServer<T>
    {
        protected List<T> Data = new();

        public void Setup()
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
            server.SendTo(Encoding.UTF8.GetBytes("Hello"), new IPEndPoint(IPAddress.Parse("224.168.100.2"), 1));
            server.Close();
        }
    }
}