using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public static class MulticastReceiver
    {
        public static void Run()
        {
            IPAddress localIp;
            using (Socket socket = new(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = (IPEndPoint)socket.LocalEndPoint!;
                localIp = endPoint.Address;
            }
            EndPoint localEndpoint = new IPEndPoint(localIp, 1);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            client.Bind(localEndpoint);
            Console.WriteLine("multicast receiver running at " + localEndpoint);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), localIp);
            client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

            byte[] buffer = new byte[1024];
            while (true)
            {
                int resBytes = client.ReceiveFrom(buffer, ref remoteIp);
                if (resBytes == 0) break;
                Console.WriteLine("message received:");
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, resBytes) + "\n");
            }
            client.Close();
        }
    }
}