using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public static class MulticastReceiver
    {
        public static void Run()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIp = IPAddress.Parse(host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString());
            EndPoint localEndpoint = new IPEndPoint(localIp, 1);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            client.Bind(localEndpoint);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), localIp);
            client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

            byte[] buffer = new byte[1024];
            while (true)
            {
                int resBytes = client.ReceiveFrom(buffer, ref remoteIp);
                if (resBytes == 0) break;
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, resBytes));
            }
            client.Close();
        }
    }
}