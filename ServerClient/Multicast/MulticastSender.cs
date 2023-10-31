using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public static class MulticastSender
    {
        public static void Send(string msg)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string localIp = host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            IPEndPoint localEndPoint = new(IPAddress.Parse(localIp), 0);

            Socket server = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(localEndPoint);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), IPAddress.Parse(localIp));
            server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            server.SendTo(Encoding.UTF8.GetBytes(msg), new IPEndPoint(IPAddress.Parse("224.168.100.2"), 1));
            server.Close();
        }
    }
}