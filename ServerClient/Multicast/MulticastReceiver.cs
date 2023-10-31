using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public class MulticastReceiver
    {
        public MulticastReceiver()
        {
            Console.WriteLine("wre ekwnlaçdf;e");
            Run();
        }
        public void Run()
        {
            while (true)
            {
                MakeRequest();
            }
        }
        public void MakeRequest()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress localIp = IPAddress.Parse(host.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString());
            EndPoint localEndpoint = new IPEndPoint(localIp, 1);
            using Socket client = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            client.Bind(localEndpoint);

            MulticastOption multicastOption = new(IPAddress.Parse("224.168.100.2"), localIp);
            client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                byte[] buffer = new byte[1024];
                int resBytes = client.ReceiveFrom(buffer, ref remoteIp);
                if (resBytes == 0) break;
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, resBytes));

            }
            client.Close();
        }
    }
}