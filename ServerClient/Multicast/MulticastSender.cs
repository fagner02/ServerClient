using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SD
{
    public static class MulticastSender
    {
        /// <summary>
        /// Envia uma mensagem para os clientes multicast
        /// </summary>
        /// <param name="msg">A mensagem a ser enviada para os clientes</param>
        public static void Send(string msg)
        {
            string localIp = RequestConfig.GetLocalIp().ToString();
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